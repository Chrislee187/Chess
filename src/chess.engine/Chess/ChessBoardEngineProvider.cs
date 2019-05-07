using chess.engine.Board;
using chess.engine.Entities;
using Microsoft.Extensions.Logging;

namespace chess.engine.Chess
{
    public class ChessBoardEngineProvider : IBoardEngineProvider<ChessPieceEntity>
    {
        private readonly ILogger<BoardEngine<ChessPieceEntity>> _boardEngineLogger;
        private readonly IRefreshAllPaths<ChessPieceEntity> _refreshAllPaths;
        private readonly IPathsValidator<ChessPieceEntity> _chessPathsValidator;

        public ChessBoardEngineProvider(
            ILogger<BoardEngine<ChessPieceEntity>> boardEngineLogger,
            IRefreshAllPaths<ChessPieceEntity> refreshAllPaths,
            IPathsValidator<ChessPieceEntity> chessPathsValidator
        )
        {
            _chessPathsValidator = chessPathsValidator;
            _refreshAllPaths = refreshAllPaths;
            _boardEngineLogger = boardEngineLogger;
        }
        public BoardEngine<ChessPieceEntity> Provide(IBoardSetup<ChessPieceEntity> boardSetup)
        {
            return new BoardEngine<ChessPieceEntity>(_boardEngineLogger,
                boardSetup,
                _chessPathsValidator,
                _refreshAllPaths);
        }
    }
}