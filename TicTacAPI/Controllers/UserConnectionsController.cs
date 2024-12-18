using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacAPI.Data;
using TicTacAPI.SignalModels;

namespace TicTacAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserConnectionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserConnectionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserConnections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserConnection>>> GetConnections()
        {
            return await _context.Connections.ToListAsync();
        }

        // GET: api/UserConnections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserConnection>> GetUserConnection(int id)
        {
            var userConnection = await _context.Connections.FindAsync(id);

            if (userConnection == null)
            {
                return NotFound();
            }

            return userConnection;
        }

        // PUT: api/UserConnections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserConnection(int id, UserConnection userConnection)
        {
            if (id != userConnection.Id)
            {
                return BadRequest();
            }

            _context.Entry(userConnection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserConnectionExists(id))
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

        // POST: api/UserConnections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserConnection>> PostUserConnection(UserConnection userConnection)
        {
            _context.Connections.Add(userConnection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserConnection", new { id = userConnection.Id }, userConnection);
        }

        // DELETE: api/UserConnections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserConnection(int id)
        {
            var userConnection = await _context.Connections.FindAsync(id);
            if (userConnection == null)
            {
                return NotFound();
            }

            _context.Connections.Remove(userConnection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserConnectionExists(int id)
        {
            return _context.Connections.Any(e => e.Id == id);
        }
    }
}
