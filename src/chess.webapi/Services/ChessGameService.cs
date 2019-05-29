using System.Linq;
using board.engine;
using board.engine.Board;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Game;
using Microsoft.Extensions.Logging;

namespace chess.webapi.Services
{
    public class ChessGameService : IChessService
    {
        private readonly ILogger<ChessGameService> _logger;
        private readonly ILogger<ChessGame> _chessGameLogger;
        private readonly IBoardEngineProvider<ChessPieceEntity> _boardEngineProvider;
        private readonly IBoardEntityFactory<ChessPieceEntity> _entityFactory;
        private readonly ICheckDetectionService _checkDetectionService;

        public ChessGameService(
            ILogger<ChessGameService> logger,
            ILogger<ChessGame> chessGameLogger,
            ICheckDetectionService  checkDetectionService,
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            IBoardEntityFactory<ChessPieceEntity> entityFactory
            )
        {
            _checkDetectionService = checkDetectionService;
            _entityFactory = entityFactory;
            _chessGameLogger = chessGameLogger;
            _boardEngineProvider = boardEngineProvider;
            _logger = logger;
        }

        public ChessWebApiResult GetNewBoard()
        {
            var game = new ChessGame(
                _chessGameLogger,
                _boardEngineProvider,
                _entityFactory,
                _checkDetectionService
            );
            var result = new ChessWebApiResult(
                game,
                game.CurrentPlayer,
                string.Empty,
                game.BoardState.GetItems((int) Colours.White).ToArray());
            return result;
        }

        public ChessWebApiResult PlayMove(string board, string move)
        {
            var game= ChessGameConvert.Deserialise(board);
            var msg = game.Move(move);
            
            return new ChessWebApiResult(
                game,
                game.CurrentPlayer,
                msg,
                game.BoardState.GetItems((int)game.CurrentPlayer).ToArray()
                );
        }

        public ChessWebApiResult GetMoves(string board)
        {
            var game = ChessGameConvert.Deserialise(board);
            var items = game.BoardState.GetItems();
            return new ChessWebApiResult(game, game.CurrentPlayer, string.Empty, items.ToArray());
        }

        public ChessWebApiResult GetMovesForPlayer(string board, Colours forPlayer)
        {
            var game = ChessGameConvert.Deserialise(board);
            var items = game.BoardState.GetItems((int) forPlayer);
            return new ChessWebApiResult(game, forPlayer, string.Empty, items.ToArray());
        }

        public ChessWebApiResult GetMovesForLocation(string board, string location)
        {
            var game = ChessGameConvert.Deserialise(board);
            var loc = location.ToBoardLocation();
            var item = game.BoardState.GetItem(loc);
            return new ChessWebApiResult(game,item.Item.Player, string.Empty, item);
        }
    }
}