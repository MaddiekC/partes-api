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
    public class TranCpartesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TranCpartesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TranCpartes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TranCparte>>> GetTranCpartes()
        {
            return await _context.TranCpartes.ToListAsync();
        }

        // GET: api/TranCpartes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TranCparte>> GetTranCparte(int id)
        {
            var tranCparte = await _context.TranCpartes.FindAsync(id);

            if (tranCparte == null)
            {
                return NotFound();
            }

            return tranCparte;
        }

        // PUT: api/TranCpartes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTranCparte(int id, TranCparte tranCparte)
        {
            if (id != tranCparte.SecParte)
            {
                return BadRequest();
            }

            _context.Entry(tranCparte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TranCparteExists(id))
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

        // POST: api/TranCpartes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TranCparte>> PostTranCparte(TranCparte tranCparte)
        {
            _context.TranCpartes.Add(tranCparte);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TranCparteExists(tranCparte.SecParte))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTranCparte", new { id = tranCparte.SecParte }, tranCparte);
        }

        // DELETE: api/TranCpartes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranCparte(int id)
        {
            var tranCparte = await _context.TranCpartes.FindAsync(id);
            if (tranCparte == null)
            {
                return NotFound();
            }

            _context.TranCpartes.Remove(tranCparte);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TranCparteExists(int id)
        {
            return _context.TranCpartes.Any(e => e.SecParte == id);
        }
    }
}
