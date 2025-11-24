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
    public class LaborsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LaborsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Labors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Labor>>> GetLabors()
        {
            return await _context.Labors.ToListAsync();
        }

        // GET: api/Labors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Labor>> GetLabor(uint id)
        {
            var labor = await _context.Labors.FindAsync(id);

            if (labor == null)
            {
                return NotFound();
            }

            return labor;
        }

        // PUT: api/Labors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLabor(uint id, Labor labor)
        {
            if (id != labor.Id)
            {
                return BadRequest();
            }

            _context.Entry(labor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaborExists(id))
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

        // POST: api/Labors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Labor>> PostLabor(Labor labor)
        {
            _context.Labors.Add(labor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLabor", new { id = labor.Id }, labor);
        }

        // DELETE: api/Labors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabor(uint id)
        {
            var labor = await _context.Labors.FindAsync(id);
            if (labor == null)
            {
                return NotFound();
            }

            _context.Labors.Remove(labor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaborExists(uint id)
        {
            return _context.Labors.Any(e => e.Id == id);
        }
    }
}
