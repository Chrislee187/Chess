using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Movement;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace chess.engine.tests.Chess.Movement.King
{
    public class ValidatorTestsBase
    {
        protected ChessBoardEngineProvider ChessBoardEngineProvider = new ChessBoardEngineProvider(
            NullLogger<BoardEngine<ChessPieceEntity>>.Instance,
            new ChessRefreshAllPaths(NullLogger<ChessRefreshAllPaths>.Instance),
            new ChessPathsValidator(new ChessPathValidator(new MoveValidationFactory<ChessPieceEntity>())));

        protected  ILogger<T> MockLogger<T>() => new Mock<ILogger<T>>().Object;
        protected  ILogger<ChessRefreshAllPaths> ChessRefreshAllPathsNullLogger 
            => MockLogger<ChessRefreshAllPaths>();
    }
}