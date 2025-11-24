using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

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
            var TranDparte = await _context.TranDpartes.AsNoTracking().FirstOrDefaultAsync(x => x.SecParte == SEC_PARTE);
            if (TranDparte == null)
            {
                return NotFound();
            }
            return TranDparte;
        }
    }
}
