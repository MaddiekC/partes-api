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
    public class RhMlotSeccionController : Controller
    {
        private readonly AppDbContext _context;

        public RhMlotSeccionController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: api/RhMlotSeccion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RhMlotseccion>>> GetRhMhacis()
        {
            return await _context.RhMlotseccions.ToListAsync();
        }
    }
}
