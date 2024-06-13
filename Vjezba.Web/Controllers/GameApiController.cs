
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vjezba.DAL; 
using Vjezba.Model;

namespace Vjezba.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GamesDbContext _dbContext;

        public GameController(GamesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _dbContext.Games.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _dbContext.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

  
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(game).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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

  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _dbContext.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return _dbContext.Games.Any(e => e.Id == id);
        }
    }
}
