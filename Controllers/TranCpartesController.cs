using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranCpartesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TranCpartesController> _logger;

        public TranCpartesController(AppDbContext context, ILogger<TranCpartesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/TranCpartes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TranCparte>>> GetTranCpartes()
        {
            var ultimoPeriodo = await _context.PeriodoLiquidacions
                .OrderByDescending(p => p.Fechainicio)
                .FirstOrDefaultAsync();

            var query = _context.TranCpartes.AsQueryable();

            if (ultimoPeriodo != null && ultimoPeriodo.Fechainicio.HasValue && ultimoPeriodo.Fechafin.HasValue)
            {
                query = query.Where(p => p.FechaParte >= ultimoPeriodo.Fechainicio.Value && p.FechaParte <= ultimoPeriodo.Fechafin.Value);
            }

            return await query.ToListAsync();
        }

        // GET: api/TranCpartes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TranCparte>> GetTranCparte(int id)
        {
            var tranCparte = await _context.TranCpartes.FindAsync(id);

            if (tranCparte == null)
            {
                return NotFound();
            }

            return tranCparte;
        }


        [HttpGet("mine")]
        public async Task<IActionResult> GetMyPartes([FromQuery] int page = 1, [FromQuery] int pageSize = 15)
        {
            var userIdClaim = User.FindFirst("UserId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return Unauthorized("UserId claim missing");
            }

            if (!long.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("UserId claim inválido: {Claim}", userIdClaim.Value);
                return Unauthorized("Invalid user id claim");
            }

            var hoy = DateTime.Now.Date;

            var periodoActual = await _context.PeriodoLiquidacions
                .Where(p => 
                    p.Fechainicio.HasValue && p.Fechafin.HasValue &&
                    hoy >= p.Fechainicio.Value.ToDateTime(TimeOnly.MinValue) &&
                    hoy <= p.Fechafin.Value.ToDateTime(TimeOnly.MaxValue) &&
                    p.Estado == "A")
                .FirstOrDefaultAsync();

            var query = _context.TranCpartes
                .AsNoTracking()
                .Where(p => p.UsuarioCreId == userId && p.Estado == "A");

            if (periodoActual != null && periodoActual.Fechainicio.HasValue && periodoActual.Fechafin.HasValue)
            {
                query = query.Where(p => p.FechaParte >= periodoActual.Fechainicio.Value
                                      && p.FechaParte <= periodoActual.Fechafin.Value);
            }

            var partes = await query
                .OrderByDescending(p => p.SecParte)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new {
                    Parte = p,
                    TotalDetalles = _context.TranDpartes.Count(d => d.SecParte == p.SecParte)
                })
                .ToListAsync();

            var resultado = partes.Select(x => {
                var p = x.Parte;
                return new
                {
                    p.SecParte,
                    p.CodHacienda,
                    p.FechaParte,
                    p.Usuarioaprob,
                    p.Codigo,
                    p.Estado,
                    p.Observacion,
                    TotalDetalles = x.TotalDetalles
                };
            });

            return Ok(resultado);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTranCparte(int id, TranCparte tranCparte)
        {
            if (id != tranCparte.SecParte)
            {
                return BadRequest();
            }

            //int userId = GetCurrentUserId();
            //tranCparte.UsuarioModId = userId;
            //tranCparte.FechaMod = DateTime.Now;

            _context.Entry(tranCparte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException)
            {
                if (!TranCparteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out int id) ? id : 0;
        }

        // POST: api/TranCpartes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TranCparte>> PostTranCparte(TranCparte tranCparte)
        {
            // 1. Obtener el ID del usuario desde el Token
            int userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized("No se pudo identificar al usuario.");

            // 2. Asignar auditoría
            tranCparte.UsuarioCreId = userId;
            tranCparte.FechaCre = DateTime.Now;

            int ultimoId = 0;
            if (await _context.TranCpartes.AnyAsync())
            {
                ultimoId = await _context.TranCpartes.MaxAsync(t => t.SecParte);
            }

            tranCparte.SecParte = ultimoId + 1;

            int ultimoCodigoPersonal = 0;
            // Buscamos si ESTE usuario ya ha hecho partes antes
            var tienePartesPrevios = await _context.TranCpartes
                .AnyAsync(t => t.UsuarioCreId == userId);

            if (tienePartesPrevios)
            {
                // Buscamos el máximo código pero SOLO de este usuario
                ultimoCodigoPersonal = await _context.TranCpartes
                    .Where(t => t.UsuarioCreId == userId)
                    .MaxAsync(t => t.Codigo) ?? 0;
            }

            // El nuevo código será su último número + 1
            tranCparte.Codigo = ultimoCodigoPersonal + 1;

            _context.TranCpartes.Add(tranCparte);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TranCparteExists(tranCparte.SecParte))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTranCparte", new { id = tranCparte.SecParte }, tranCparte);
        }

        // DELETE: api/TranCpartes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranCparte(int id)
        {
            var tranCparte = await _context.TranCpartes.FindAsync(id);
            if (tranCparte == null)
            {
                return NotFound();
            }

            _context.TranCpartes.Remove(tranCparte);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TranCparteExists(int id)
        {
            return _context.TranCpartes.Any(e => e.SecParte == id);
        }

        [AllowAnonymous]
        // PATCH: api/TranCpartes/Desactivar/5
        [HttpPatch("Desactivar/{SEC_PARTE}")]
        public async Task<IActionResult> DesactivarParte(int SEC_PARTE)
        {
            // 1. Buscar la cabecera
            var parte = await _context.TranCpartes.FindAsync(SEC_PARTE);

            if (parte == null)
            {
                return NotFound(new { message = $"No se encontró el parte con ID {SEC_PARTE}" });
            }
            parte.Estado = "I";

            try
            {
                _context.Entry(parte).Property(x => x.Estado).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error de concurrencia al desactivar.");
            }

            return Ok(new { message = "Parte desactivado correctamente", SECPARTE = SEC_PARTE });
        }
    }
}
