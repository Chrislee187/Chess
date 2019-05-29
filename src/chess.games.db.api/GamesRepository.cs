using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chess.games.db.Entities;
using chess.pgn;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace chess.games.db.api
{
    public interface IGamesRepository
    {
        bool Exists(PgnGame pgnGame);
        Game GetOrCreate(PgnGame pgnGame);
        IEnumerable<Game> Select();
    }

    public class GamesRepository : IGamesRepository
    {
        private readonly ChessGamesDbContext _chessGamesDbContext;

        private bool PgnGameMatcher(PgnGame pgnGame, Game game)
        {
            return game.Black.Name.Equals(pgnGame.Black)
                   && game.White.Name.Equals(pgnGame.White)
                   && game.Event.Name.Equals(pgnGame.Event)
                   && game.Site.Name.Equals(pgnGame.Site)
                   && game.Round.Equals(pgnGame.Round)
                   && game.MoveText.Equals(pgnGame.MoveText)
                   && game.Date.Equals(pgnGame.Date.ToString())
                //                Result = pgnGame.Result    // TODO: Mappers
                ;
        }

        private IIncludableQueryable<Game, DbEntity> HydrateGames(DbSet<Game> games)
        {
            return games.Include(i => i.Black)
                .Include(i => i.White)
                .Include(i => i.Event)
                .Include(i => i.Site);

        }

        public GamesRepository(ChessGamesDbContext chessGamesDbContext)
        {
            _chessGamesDbContext = chessGamesDbContext;
        }

        public  bool Exists(PgnGame pgnGame) =>
            HydrateGames(_chessGamesDbContext.Games)
                .Any(game => PgnGameMatcher(pgnGame, game));

        private Game FindExact(PgnGame pgnGame) =>
            // TODO: NEED TO SORT THE TOLIST THING, EVERY NEW GAME ADDED MAKES ADDING THE NEXT ONE SLOWER
            HydrateGames(_chessGamesDbContext.Games).ToList() // NOTE: This only works when ToList() is here, don't think we want to read the whole DB every time?
                .SingleOrDefault(game => PgnGameMatcher(pgnGame, game));

        public Game GetOrCreate(PgnGame pgnGame)
        {
            var game = FindExact(pgnGame);

            if (game != null) return game;

            var site = GetOrCreateSite(pgnGame);
            var whitePlayer = GetOrCreatePlayer(pgnGame.White);
            var blackPlayer = GetOrCreatePlayer(pgnGame.Black);
            var @event = GetOrCreateEvent(pgnGame);

            game = new Game
            {
                Event = @event,
                Black = blackPlayer,
                Site = site,
                White = whitePlayer,
                Round = pgnGame.Round,
                MoveText = pgnGame.MoveText,
                Date = pgnGame.Date.ToString(),
                //                Result = pgnGame.Result // TODO: Mappers
            };
            _chessGamesDbContext.Games.Add(game);
            _chessGamesDbContext.SaveChanges();
            return game;
        }

        public  IEnumerable<Game> Select(){
            return _chessGamesDbContext.Games;
        }

        private Site GetOrCreateSite(PgnGame pgnGame)
        {
            Site site;
            if (_chessGamesDbContext.Sites.Any(s => s.Name.Equals(pgnGame.Site)))
            {
                site = _chessGamesDbContext.Sites.Single(s => s.Name.Equals(pgnGame.Site));
            }
            else
            {
                site = new Site { Name = pgnGame.Site };
//                _chessGamesDbContext.Sites.Add(site);
            }

            return site;
        }

        private Event GetOrCreateEvent(PgnGame pgnGame)
        {
            Event evt;
            if (_chessGamesDbContext.Events.Any(s => s.Name.Equals(pgnGame.Event)))
            {
                evt = _chessGamesDbContext.Events.Single(s => s.Name.Equals(pgnGame.Event));
            }
            else
            {
                evt = new Event { Name = pgnGame.Event };
//                _chessGamesDbContext.Events.Add(evt);
            }

            return evt;
        }

        private  Player GetOrCreatePlayer(string playerName)
        {
            Player player;
            if (_chessGamesDbContext.Players.Any(s => s.Name.Equals(playerName)))
            {
                player = _chessGamesDbContext.Players.Single(s => s.Name.Equals(playerName));
            }
            else
            {
                player = new Player { Name = playerName };
//                _chessGamesDbContext.Players.Add(player);
            }

            return player;
        }
    }
}
