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

        // GET: api/AdMusuas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RhMtrab>>> GetRhMhacis()
        {
            return await _context.RhMtrabs.ToListAsync();
        }
        //GET: api/RhMhacis/5
        [HttpGet("{COD_TRABJ}")]
        public async Task<ActionResult<RhMtrab>> GetRhMtrab(uint COD_TRABJ)
        {
            var RhMtrab = await _context.RhMtrabs.AsNoTracking().FirstOrDefaultAsync(x => x.CodTrabaj == COD_TRABJ);
            if (RhMtrab == null)
            {
                return NotFound();
            }
            return RhMtrab;
        }
    }
}
