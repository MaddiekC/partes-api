using Dapper; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; 
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using PartesApi.Data;
using PartesApi.Models;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdMusuasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration; // Inyecta IConfiguration

        public AdMusuasController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; // Asigna la instancia inyectada
        }

        // GET: api/AdMusuas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdMusua>>> GetAdMusuas()
        {
            return await _context.AdMusuas.ToListAsync();
        }

        // GET: api/AdMusuas/5
        [HttpGet("{US_CODIGO}")]
        public async Task<ActionResult<AdMusua>> GetAdMusua(uint US_CODIGO)
        {
            var adMusua = await _context.AdMusuas.AsNoTracking().FirstOrDefaultAsync(x => x.UsCodigo == US_CODIGO);
            if (adMusua == null)
            {
                return NotFound();
            }
            return adMusua;
        }
        // Post: api/AdMusuas
        public class LoginDto
        {
            public required string Login { get; set; }
            public required string Passw { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Passw))
                return BadRequest("US_LOGIN y US_PASSW no pueden ser nulos.");

            var user = await _context.AdMusuas
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UsLogin == dto.Login && u.UsPassw == dto.Passw);

            if (user == null) return Unauthorized(new { msg = "Usuario o contraseña incorrectos." });

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UsLogin ?? string.Empty),
                new Claim("UserId", user.UsCodigo?.ToString() ?? string.Empty),
                new Claim("Name", user.UsNombre?.ToString() ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtSection = _configuration.GetSection("Jwt"); // Usa la instancia inyectada
            var jwtKey = jwtSection["Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                return StatusCode(StatusCodes.Status500InternalServerError, "La clave JWT no está configurada correctamente.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpireMinutes"] ?? "60")),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString, msg = "Inicio de sesión exitoso" });

        }


        public class EsquemaDto
        {
            public string Nombre { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
            public string Extra { get; set; } = string.Empty; // Para detectar llaves primarias o autoincrementales
        }

        [HttpGet("esquema/{nombreTabla}")]
        public async Task<IActionResult> GetEsquema(string nombreTabla)
        {
            // Consulta específica para MySQL
            var sql = @"SELECT COLUMN_NAME AS Nombre, 
                       DATA_TYPE AS Tipo, 
                       COLUMN_KEY AS Extra 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = {0} 
                AND TABLE_SCHEMA = DATABASE()";

            var esquema = await _context.Database
                .SqlQueryRaw<EsquemaDto>(sql, nombreTabla)
                .ToListAsync();

            if (!esquema.Any()) return NotFound($"La tabla {nombreTabla} no existe.");

            return Ok(esquema);
        }


        [HttpGet("datos/{nombreTabla}")]
        public async Task<IActionResult> GetDatosTabla(string nombreTabla)
        {
            var tablasPermitidas = new[] { "rh_mhaci", "rh_mlotes", "labor", "rh_mtrab", "det_asistencia", "unidad_medida", "rh_mlotseccion", "area_grupo_labor" , "rh_mlotseccion", "falta_empleado", "parametro", "periodo_liquidacion", "tran_cparte", "tran_dparte"};
            if (!tablasPermitidas.Contains(nombreTabla.ToLower()))
                return BadRequest("Tabla no permitida.");

            using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                var sql = $"SELECT * FROM {nombreTabla} WHERE ESTADO IN ('A', '2')";
                // Filtro especial 
                if (nombreTabla.ToLower() == "det_asistencia")
                {
                    sql = "SELECT * FROM det_asistencia WHERE fecha >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)";
                }

                //PARA FALTAS
                if (nombreTabla.ToLower() == "falta_empleado")
                {
                    sql = "SELECT * FROM falta_empleado WHERE fecha >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)";
                }

                //PARA PERIODO
                if (nombreTabla.ToLower() == "periodo_liquidacion")
                {
                    sql = @"SELECT * FROM periodo_liquidacion 
                            WHERE CURDATE() BETWEEN fechainicio AND fechafin 
                            AND estado = 'A' 
                            LIMIT 1";
                }

                if (nombreTabla.ToLower() == "tran_cparte")
                {
                    sql = @"SELECT sec_parte AS id_local, codigo, cod_hacienda, fecha_parte, estado, observacion 
                            FROM tran_cparte 
                            WHERE fecha_parte BETWEEN ( 
                                SELECT fechainicio FROM periodo_liquidacion 
                                WHERE CURDATE() BETWEEN fechainicio AND fechafin 
                                AND estado = 'A' LIMIT 1
                            ) AND(
                                SELECT fechafin FROM periodo_liquidacion
                                WHERE CURDATE() BETWEEN fechainicio AND fechafin
                                AND estado = 'A' LIMIT 1
                            )";
                }
                if (nombreTabla.ToLower() == "tran_dparte")
                {
                    sql = @"SELECT sec_parte, secuencia, cod_trabaj AS cod_trabj, cod_labor, lote_id, nom_seccion, cantidad, fecha_inicio, fecha_fin 
                            FROM tran_dparte 
                            WHERE sec_parte IN (
                            SELECT sec_parte FROM tran_cparte
                                WHERE fecha_parte BETWEEN (SELECT fechainicio FROM periodo_liquidacion  
                                WHERE CURDATE() BETWEEN fechainicio AND fechafin 
                                AND estado = 'A' LIMIT 1
                            ) AND (
                                SELECT fechafin FROM periodo_liquidacion
                                WHERE CURDATE() BETWEEN fechainicio AND fechafin
                                AND estado = 'A' LIMIT 1
                                  ) 
                            )";
                }

                var datosRaw = await connection.QueryAsync(sql);

                // Convertimos cada fila a un diccionario y filtramos valores complejos
                var datosLimpios = datosRaw.Select(row =>
                {
                    var dict = (IDictionary<string, object>)row;
                    return dict.ToDictionary(
                        kvp => kvp.Key,
                    kvp => {
                            // Si el valor es una fecha o un objeto complejo, lo convertimos a texto
                            if (kvp.Value is DateTime dt) return dt.ToString("yyyy -MM-dd");

                            if (kvp.Value != null && kvp.Value.GetType().Name.Contains("DateTime"))
                                return Convert.ToDateTime(kvp.Value.ToString()).ToString("yyyy-MM-dd");

                            if (kvp.Value != null && !kvp.Value.GetType().IsValueType && !(kvp.Value is string))
                                return null;

                            return kvp.Value;
                        }
                    );
                });

                return Ok(datosLimpios);
            }
        }
    }
    }
