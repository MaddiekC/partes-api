using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdMusuasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdMusuasController(AppDbContext context)
        {
            _context = context;
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
    }
}
