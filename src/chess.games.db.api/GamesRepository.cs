using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using chess.games.db.Entities;
using chess.pgn;

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

        public GamesRepository(ChessGamesDbContext chessGamesDbContext)
        {
            _chessGamesDbContext = chessGamesDbContext;
        }

        public  bool Exists(PgnGame pgnGame) =>
            ChessGamesDbContext.HydrateGames(_chessGamesDbContext.Games)
                .Any(game => PgnGameMatcher(pgnGame, game));

        private Game FindExact(PgnGame pgnGame) =>
            ChessGamesDbContext.HydrateGames(_chessGamesDbContext.Games) 
                .SingleOrDefault(game => PgnGameMatcher(pgnGame, game));

        public Game GetOrCreate(PgnGame pgnGame)
        {
            var game = FindExact(pgnGame);

            if (game != null) return game;

            var site = _chessGamesDbContext.GetOrCreate(
                s1 => s1.Name.Equals(pgnGame.Site),
                () => new Site {Name = pgnGame.Site});
            var whitePlayer = _chessGamesDbContext.GetOrCreate(
                s1 => s1.Name.Equals(pgnGame.White),
                () => new Player { Name = pgnGame.White});
            var blackPlayer = _chessGamesDbContext.GetOrCreate(
                s1 => s1.Name.Equals(pgnGame.Black),
                () => new Player { Name = pgnGame.Black });
            var @event = _chessGamesDbContext.GetOrCreate(
                s1 => s1.Name.Equals(pgnGame.Event),
                () => new Event { Name = pgnGame.Event });

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

    }
}
