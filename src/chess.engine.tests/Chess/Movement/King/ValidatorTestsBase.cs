using chess.engine.Chess;
using Microsoft.Extensions.Logging;
using Moq;

namespace chess.engine.tests.Chess.Movement.King
{
    public class ValidatorTestsBase
    {
        protected  ILogger<T> MockLogger<T>() => new Mock<ILogger<T>>().Object;
        protected  ILogger<ChessRefreshAllPaths> ChessRefreshAllPathsNullLogger 
            => MockLogger<ChessRefreshAllPaths>();
    }
}