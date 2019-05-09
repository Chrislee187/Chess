using System.Linq;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using Microsoft.Extensions.Logging;

namespace chess.webapi.Services
{
    public class ChessGameService : IChessService
    {
        private ILogger<ChessGameService> _logger;
        private readonly ILogger<ChessGame> _chessGameLogger;
        private readonly IBoardEngineProvider<ChessPieceEntity> _boardEngineProvider;

        public ChessGameService(
            ILogger<ChessGameService> logger,
            ILogger<ChessGame> chessGameLogger, 
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider)
        {
            _chessGameLogger = chessGameLogger;
            _boardEngineProvider = boardEngineProvider;
            _logger = logger;
        }

        public ChessGameResult GetNewBoard()
        {
            var game = new ChessGame(
                _chessGameLogger,
                _boardEngineProvider
            );
            var result = new ChessGameResult(game, game.BoardState.GetAllItems().ToArray());
            return result;
        }

        public ChessGameResult PlayMove(string board, string move)
        {
            var game= CreateChessGame(board);
            var msg = game.Move(move);
            return new ChessGameResult(game, msg);
        }

        public ChessGameResult GetMoves(string board)
        {
            var game = CreateChessGame(board);
            var items = game.BoardState.GetAllItems();
            return new ChessGameResult(game, items.ToArray());
        }

        public ChessGameResult GetMovesForPlayer(string board, Colours forPlayer)
        {
            var game = CreateChessGame(board);
            var items = game.BoardState.GetItems((int) forPlayer);
            return new ChessGameResult(game, items.ToArray());
        }

        public ChessGameResult GetMovesForLocation(string board, string location)
        {
            var game = CreateChessGame(board);
            var loc = BoardLocation.At(location);
            var item = game.BoardState.GetItem(loc);
            return new ChessGameResult(game, item);
        }


        private static ChessGame CreateChessGame(string board)
            => ChessGameConvert.Deserialise(board);
    }
}