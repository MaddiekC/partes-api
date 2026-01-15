using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetAsistenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DetAsistenciasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UndiadMedidas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetAsistencia>>> GetDetAsistencias()
        {
            return await _context.DetAsistencias.ToListAsync();
        }
    }
}
