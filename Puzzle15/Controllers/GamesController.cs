using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puzzle15.Models;

namespace Puzzle15.Controllers
{
    [Produces("application/json")]
    [Route("api/Games")]
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public IEnumerable<Game> GetGames()
        {
            return _context.Games;
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _context.Games.SingleOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // POST: api/Games/New
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> NewGame()
        {
            var game = new Game();
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // PUT api/Games/Move?gameId={gameId}&tileNum={tileNum}
        [HttpPut]
        [Route("Move")]
        public async Task<IActionResult> MoveTile()
        {
            var gameId = Request.Query["gameId"].ToString();
            var tileNum = Request.Query["tileNum"].ToString();

            if (gameId.Length == 0 || tileNum.Length == 0)
                return BadRequest();

            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == Convert.ToInt32(gameId));

            if (game == null)
                return NotFound();

            try
            {
                game.MoveTile(Convert.ToInt32(tileNum));
                
            }
            catch (Exception e)
            {
                return BadRequest();
            } 
            
            _context.Entry(game).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _context.Games.SingleOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return Ok(game);
        }
    }
}