using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudRegistrosController : Controller
    {
        private readonly AppDbContext _context;

        public AudRegistrosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AudRegistro>>> GetAudRegistros()
        {
            return await _context.AudRegistros.ToListAsync();
        }
    }
}
