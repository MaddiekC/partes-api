using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PartesApi.Data;
using PartesApi.Models;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration; // Añade este using si no está

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

            // generar claims (añade lo que necesites)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UsLogin ?? string.Empty),
                new Claim("UserId", user.UsCodigo?.ToString() ?? string.Empty),
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
    }
}
