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
    public class RhMlotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RhMlotesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RhMlotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RhMlote>>> GetRhMlotes()
        {
            return await _context.RhMlotes.ToListAsync();
        }

        // GET: api/RhMlotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RhMlote>> GetRhMlote(int id)
        {
            var rhMlote = await _context.RhMlotes.FindAsync(id);

            if (rhMlote == null)
            {
                return NotFound();
            }

            return rhMlote;
        }

        // PUT: api/RhMlotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRhMlote(int id, RhMlote rhMlote)
        {
            if (id != rhMlote.LoteId)
            {
                return BadRequest();
            }

            _context.Entry(rhMlote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RhMloteExists(id))
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

        // POST: api/RhMlotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RhMlote>> PostRhMlote(RhMlote rhMlote)
        {
            _context.RhMlotes.Add(rhMlote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRhMlote", new { id = rhMlote.LoteId }, rhMlote);
        }

        // DELETE: api/RhMlotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRhMlote(int id)
        {
            var rhMlote = await _context.RhMlotes.FindAsync(id);
            if (rhMlote == null)
            {
                return NotFound();
            }

            _context.RhMlotes.Remove(rhMlote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RhMloteExists(int id)
        {
            return _context.RhMlotes.Any(e => e.LoteId == id);
        }
    }
}
