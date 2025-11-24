using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RhMhacisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RhMhacisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AdRhMhacis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RhMhaci>>> GetRhMhacis()
        {
            return await _context.RhMhacis.ToListAsync();
        }

        //GET: api/RhMhacis/5
        [HttpGet("{COD_HACIENDA}")]
        public async Task<ActionResult<RhMhaci>> GetRhMhaci(uint COD_HACIENDA)
        {
            var rhMhaci = await _context.RhMhacis.AsNoTracking().FirstOrDefaultAsync(x => x.CodHacienda == COD_HACIENDA);
            if (rhMhaci == null)
            {
                return NotFound();
            }
            return rhMhaci;
        }
    }
}
