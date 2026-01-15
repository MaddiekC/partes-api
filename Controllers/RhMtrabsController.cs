using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RhMtrabsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RhMtrabsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RhMtrabs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RhMtrab>>> GetRhMhacis()
        {
            return await _context.RhMtrabs.ToListAsync();
        }
        //GET: api/RhMtrabs/5
        [HttpGet("{fecha}")]
        public async Task<ActionResult<RhMtrab>> GetRhMtrab(DateOnly fecha)
        {
            var codigosAsistencia = await _context.DetAsistencias
                .Where(a => a.Fecha.HasValue && a.Fecha.Value == fecha)
                .Select(a => a.CodTrabaj)
                .Distinct()
                .ToListAsync();

            var RhMtrab = await _context.RhMtrabs
                .Where(t => codigosAsistencia.Contains(t.CodTrabaj))
                .OrderBy(t => t.NombreCorto)
                .AsNoTracking()
                .ToListAsync();

            if (RhMtrab == null)
            {
                return NotFound();
            }
            return Ok(RhMtrab);
        }
    }
}
