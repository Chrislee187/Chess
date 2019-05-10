using board.engine;
using board.engine.Actions;
using board.engine.Board;
using chess.engine.Chess.Entities;
using Microsoft.Extensions.Logging;

namespace chess.engine.Chess
{
    public class ChessBoardEngineProvider : IBoardEngineProvider<ChessPieceEntity>
    {
        private readonly ILogger<BoardEngine<ChessPieceEntity>> _boardEngineLogger;
        private readonly IRefreshAllPaths<ChessPieceEntity> _refreshAllPaths;
        private readonly IPathsValidator<ChessPieceEntity> _chessPathsValidator;
        private readonly IBoardActionProvider<ChessPieceEntity> _actionProvider;

        public ChessBoardEngineProvider(
            ILogger<BoardEngine<ChessPieceEntity>> boardEngineLogger,
            IRefreshAllPaths<ChessPieceEntity> refreshAllPaths,
            IPathsValidator<ChessPieceEntity> chessPathsValidator,
            IBoardActionProvider<ChessPieceEntity> actionProvider
        )
        {
            _actionProvider = actionProvider;
            _chessPathsValidator = chessPathsValidator;
            _refreshAllPaths = refreshAllPaths;
            _boardEngineLogger = boardEngineLogger;
        }
        public BoardEngine<ChessPieceEntity> Provide(IBoardSetup<ChessPieceEntity> boardSetup)
        {
            return new BoardEngine<ChessPieceEntity>(_boardEngineLogger,
                boardSetup,
                _chessPathsValidator,
                _actionProvider,
                _refreshAllPaths);
        }
    }
}