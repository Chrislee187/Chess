using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using chess.games.db.Entities;

namespace chess.web.Pages.Pgn
{
    public class Library2Model : PageModel
    {
        private readonly chess.games.db.Entities.ChessGamesDbContext _context;

        public Library2Model(chess.games.db.Entities.ChessGamesDbContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; }

        public async Task OnGetAsync()
        {
            Game = _context.GamesWithIncludes()
                .Where(g => g.ContainsPlayer("Short"))
                .OrderBy(g => g.Date)
                .ThenBy(g => g.Site.Name)
                .ThenBy(g => g.Event.Name)
                .ThenBy(g => g.Round)
                .ThenBy(g => g.White.Name)
                .ThenBy(g => g.Black.Name)
                .ToList();
        }
    }
}
