using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadMedidasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UnidadMedidasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UndiadMedidas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnidadMedida>>> GetUnidadMedidas()
        {
            return await _context.UnidadMedidas.ToListAsync();
        }
    }
}
