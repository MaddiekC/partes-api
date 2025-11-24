using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartesApi.Data;
using PartesApi.Models;

namespace PartesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarifasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarifasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tarifas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarifa>>> GetTarifas()
        {
            return await _context.Tarifas.ToListAsync();
        }

        // GET: api/Tarifas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarifa>> GetTarifa(uint id)
        {
            var tarifa = await _context.Tarifas.FindAsync(id);

            if (tarifa == null)
            {
                return NotFound();
            }

            return tarifa;
        }

        // PUT: api/Tarifas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarifa(uint id, Tarifa tarifa)
        {
            if (id != tarifa.IdLabor)
            {
                return BadRequest();
            }

            _context.Entry(tarifa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarifaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tarifas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tarifa>> PostTarifa(Tarifa tarifa)
        {
            _context.Tarifas.Add(tarifa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarifa", new { id = tarifa.IdLabor }, tarifa);
        }

        // DELETE: api/Tarifas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarifa(uint id)
        {
            var tarifa = await _context.Tarifas.FindAsync(id);
            if (tarifa == null)
            {
                return NotFound();
            }

            _context.Tarifas.Remove(tarifa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TarifaExists(uint id)
        {
            return _context.Tarifas.Any(e => e.IdLabor == id);
        }
    }
}
