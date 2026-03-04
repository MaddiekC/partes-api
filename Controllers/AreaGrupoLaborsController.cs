using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaGrupoLaborsController : Controller
    {
        private readonly AppDbContext _context;
        public AreaGrupoLaborsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AreaGrupoLabor>>> GetAreaGrupoLabors()
        {
            return await _context.AreaGrupoLabors.ToListAsync();
        }
    }
}
