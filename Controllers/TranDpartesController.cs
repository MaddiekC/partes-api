using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PartesApi.Data;
using PartesApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranDpartesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TranDpartesController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/AdRhMhacis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TranDparte>>> GetRhMhacis()
        {
            return await _context.TranDpartes.ToListAsync();
        }

        //GET: api/RhMhacis/5
        [HttpGet("{SEC_PARTE}")]
        public async Task<ActionResult<TranDparte>> GetRhMhaci(uint SEC_PARTE)
        {
            var TranCparte = await _context.TranCpartes
                .AsNoTracking()
                .Select(c => new { c.SecParte, c.Usuarioaprob })
                .FirstOrDefaultAsync(c => c.SecParte == SEC_PARTE);

            var TranDparte = await _context.TranDpartes
                .AsNoTracking()
                .Where(x => x.SecParte == SEC_PARTE)
                .ToListAsync();

            return Ok(new
            {
                aprobado = TranCparte != null && TranCparte.Usuarioaprob.HasValue,
                usuarioAprob = TranCparte?.Usuarioaprob,
                detalles = TranDparte
            });
        }

        public class TranDparteCreateDto
        {
            public int SecParte { get; set; }
            public List<TranDparteDetalleDto> Detalles { get; set; } = new();
        }

        public class TranDparteDetalleDto
        {
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public decimal Cantidad { get; set; }
            public int CodTrabaj { get; set; } 
            public int Lote { get; set; }
            public int Labor { get; set; }
            public int Secuencia { get; set; }
            public String NomSeccion { get; set; }
            public int ValorUnitario { get; set; }
            public int ValorTotal { get; set; }
        }


        //POST: api/RhMhacis

        [HttpPost]
        public async Task<IActionResult> CreateDetalles([FromBody] TranDparteCreateDto dto)
        {
            if (dto == null || dto.Detalles == null || dto.Detalles.Count == 0)
                return BadRequest("No se enviaron detalles");

            var codigosLabor = dto.Detalles.Select(d => (uint)d.Labor).Distinct().ToList();

            var laboresInfo = await _context.Labors
                .Where(l => codigosLabor.Contains(l.Id))
                .ToDictionaryAsync(l => l.Id);

            var tarifasDict = await _context.Tarifas
                .Where(t => codigosLabor.Contains((uint)t.IdLabor) && t.Estado == "A")
                .ToDictionaryAsync(t => (uint)t.IdLabor, t => (double)t.Valor);

            foreach (var d in dto.Detalles)
            {
                var fechaActual = DateOnly.FromDateTime(d.FechaInicio);

                // 1. BUSCAR ACUMULADO: Sumamos lo que ya existe en la DB para este empleado, labor y fecha
                var acumuladoEnDB = await _context.TranDpartes
                    .Where(x => x.CodTrabaj == d.CodTrabaj
                             && x.CodLabor == d.Labor
                             && x.FechaInicio == fechaActual)
                    .SumAsync(x => x.Cantidad);

                double totalHoy = (double)(acumuladoEnDB + d.Cantidad);

                if (laboresInfo.TryGetValue((uint)d.Labor, out var labor))
                {
                    // Validación de Máximo
                    if (totalHoy > (double)labor.AvanceMaximo)
                    {
                        return BadRequest($"El empleado {d.CodTrabaj} ya tiene {acumuladoEnDB} registrados. " +
                                         $"Con este nuevo ingreso suma {totalHoy}, excediendo el máximo de {labor.AvanceMaximo}.");
                    }

                    // Validación de Mínimo
                    if (d.Cantidad < labor.AvanceMinimo)
                    {
                        return BadRequest($"Error: El empleado {d.CodTrabaj} no alcanza la cantidad mínima ({labor.AvanceMinimo}) para la labor {labor.Nombre}");
                    }
                }
                else
                {
                    return BadRequest($"Labor con código {d.Labor} no encontrada.");
                }
            }

            var entidades = dto.Detalles.Select(d =>
            {
                var valorUnitario = (decimal)(tarifasDict.TryGetValue((uint)d.Labor, out var tarifa) ? tarifa : 0);
                return new TranDparte
                {
                    SecParte = dto.SecParte,
                    Secuencia = d.Secuencia,
                    CodTrabaj = d.CodTrabaj,
                    LoteId = d.Lote,
                    CodLabor = d.Labor,
                    NomSeccion = d.NomSeccion.ToString(),
                    FechaInicio = DateOnly.FromDateTime(d.FechaInicio),
                    FechaFin = DateOnly.FromDateTime(d.FechaFin),
                    Cantidad = d.Cantidad,
                    ValorUnitario = valorUnitario,
                    ValorTotal = valorUnitario * d.Cantidad
                };
            }).ToList();

            await _context.TranDpartes.AddRangeAsync(entidades);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Detalles creados correctamente",
                total = entidades.Count
            });
        }

        public class TranDparteUpdateDto
        {
            public int SecParte { get; set; }
            public int Secuencia { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public decimal Cantidad { get; set; }
            public int CodTrabaj { get; set; }
            public int Lote { get; set; }
            public String NomSeccion { get; set; }
            public int Labor { get; set; }
            public int ValorUnitario { get; set; }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out int id) ? id : 0;
        }
        private string GetCurrentUserName()
        {
            var userNameClaim = User.FindFirst("Name")?.Value;
            return userNameClaim ?? string.Empty;
        }

        [AllowAnonymous]
        [HttpPut("{secParte}/{secuencia}")]
        public async Task<IActionResult> UpdateDetalle(int secParte, int secuencia, [FromBody] TranDparteUpdateDto dto)
        {
            if (secParte != dto.SecParte || secuencia != dto.Secuencia)
            {
                return BadRequest("Los identificadores de la URL no coinciden con los del cuerpo de la solicitud.");
            }

            // 1. Buscar el registro existente
            var detalleExistente = await _context.TranDpartes
                .FirstOrDefaultAsync(x => x.SecParte == secParte && x.Secuencia == secuencia);

            if (detalleExistente == null)
            {
                return NotFound($"No se encontró el detalle con SecParte {secParte} y Secuencia {secuencia}.");
            }

            // 2. Validar límites de Labor
            var labor = await _context.Labors.FindAsync((uint)dto.Labor);
            if (labor == null) return BadRequest("Labor no encontrada.");


            // 3. Validar acumulado (restando la cantidad anterior del mismo registro)
            var fechaActual = DateOnly.FromDateTime(dto.FechaInicio);
            var acumuladoEnDB = await _context.TranDpartes
                .Where(x => x.CodTrabaj == dto.CodTrabaj
                         && x.CodLabor == dto.Labor
                         && x.FechaInicio == fechaActual
                         && !(x.SecParte == secParte && x.Secuencia == secuencia)) 
                .SumAsync(x => x.Cantidad) ?? 0;

            decimal totalHoy = acumuladoEnDB + dto.Cantidad;
             
            if (totalHoy > labor.AvanceMaximo)
            {
                return BadRequest($"El empleado ya tiene {acumuladoEnDB} registrados. " +
                                 $"Con la edición suma {totalHoy}, excediendo el máximo de {labor.AvanceMaximo}.");
            }

            if (dto.Cantidad < labor.AvanceMinimo)
            {
                return BadRequest($"La cantidad es inferior al mínimo permitido ({labor.AvanceMinimo}).");
            }

            var cambios = new StringBuilder();
            cambios.Append($"Empleado: {dto.CodTrabaj}; labor: {dto.Labor}; Sec_parte: {secParte}; Linea: {secuencia} ");

            bool huboCambios = false;

            // Comparar Lote
            if (detalleExistente.LoteId != dto.Lote)
            {
                cambios.Append($"Campo LOTE, antes: {detalleExistente.LoteId}, despues: {dto.Lote}; ");
                huboCambios = true;
            }

            // Comparar Labor
            if (detalleExistente.CodLabor != dto.Labor)
            {
                cambios.Append($"Campo LABOR, antes: {detalleExistente.CodLabor}, despues: {dto.Labor}; ");
                huboCambios = true;
            }

            // Comparar Cantidad
            if ((decimal)(detalleExistente.Cantidad ?? 0) != dto.Cantidad)
            {
                cambios.Append($"Campo CANTIDAD, antes: {detalleExistente.Cantidad}, despues: {dto.Cantidad}; ");
                huboCambios = true;
            }

            if (detalleExistente.NomSeccion != dto.NomSeccion.ToString())
            {
                cambios.Append($"Campo NOM_SECCION, antes: {detalleExistente.NomSeccion}, despues: {dto.NomSeccion}; ");
                huboCambios = true;
            }

            string scriptFinal = cambios.ToString();
            try
            {
                decimal nuevoValorTotal = dto.Cantidad * (detalleExistente.ValorUnitario ?? 0);
                var filasAfectadas = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE tran_dparte 
                    SET cod_trabaj = {dto.CodTrabaj}, 
                        cod_labor = {dto.Labor}, 
                        cantidad = {dto.Cantidad}, 
                        lote_id = {dto.Lote},
                        nom_seccion = {dto.NomSeccion},
                        fecha_inicio = {DateOnly.FromDateTime(dto.FechaInicio)}, 
                        fecha_fin = {DateOnly.FromDateTime(dto.FechaFin)},
                        valor_total = {nuevoValorTotal}
                    WHERE sec_parte = {secParte} AND secuencia = {secuencia}");

                if (filasAfectadas > 0)
                {
                    int userId = GetCurrentUserId();
                    var nombreUsuario = GetCurrentUserName();

                    await _context.Database.ExecuteSqlInterpolatedAsync($@"
                          INSERT INTO aud_registros (fechacre, usuariocre, maquina, script)
                          VALUES (NOW(), {userId}, {nombreUsuario}, {scriptFinal})");
                }
                else
                {
                    return NotFound("No se encontró el registro para actualizar.");
                }         

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en la base de datos: {ex.Message}");
            }

            return Ok(new { message = "Detalle actualizado correctamente" });
        }
    }
}
