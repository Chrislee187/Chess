using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess;
using chess.engine.Chess.Actions;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Movement;
using chess.engine.Game;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace chess.engine.tests.Builders
{
    public class ChessGameBuilder
    {
        public ILogger<T> Logger<T>() => CreateNullLogger<T>();

        private static ILogger<T> CreateMockLogger<T>()
            => new Mock<ILogger<T>>().Object;
        private static ILogger<T> CreateNullLogger<T>()
            => NullLogger<T>.Instance;

        public IRefreshAllPaths<ChessPieceEntity> BuildRefreshAllPaths()
        {
            return new ChessRefreshAllPaths(
                Logger<ChessRefreshAllPaths>(),
                BuildActionFactory(),
                new ChessGameStateService(NullLogger<ChessGameStateService>.Instance
                )
            );
        }


        public IBoardActionProvider<ChessPieceEntity> BuildActionFactory()
        {
            return new ChessBoardActionProvider(BuildEntityFactory());
        }
        public IMoveValidationProvider<ChessPieceEntity> BuildValidationFactory()
        {
            return new ChessMoveValidationProvider();
        }

        public IPathValidator<ChessPieceEntity> BuildPathValidator()
        {
            return new ChessPathValidator(
                Logger<ChessPathValidator>(),
                BuildValidationFactory());
        }
        public IPathsValidator<ChessPieceEntity> BuildPathsValidator()
        {
            return new ChessPathsValidator(
                Logger<ChessPathsValidator>(),
                BuildPathValidator()
            );
        }

        public IBoardEngineProvider<ChessPieceEntity> BuildEngine()
        {
            return new ChessBoardEngineProvider(
                Logger<BoardEngine<ChessPieceEntity>>(),
                BuildRefreshAllPaths(),
                BuildPathsValidator(),
                BuildActionFactory()
            );
        }

        public ChessBoardEngineProvider BuildEngineProvider()
        {
            return new ChessBoardEngineProvider(
                Logger<BoardEngine<ChessPieceEntity>>(),
                BuildRefreshAllPaths(),
                BuildPathsValidator(),
                BuildActionFactory()
            );
        }
        public IChessGameStateService BuildGameStateService()
        {
            return new ChessGameStateService(Logger<ChessGameStateService>());
        }

        public ChessPieceEntityFactory BuildEntityFactory()
        {
            return new ChessPieceEntityFactory();
        }

        public ChessGame BuildGame()
        {
            return new ChessGame(
                    Logger<ChessGame>(),
                    BuildEngineProvider(),
                    BuildEntityFactory(),
                    BuildGameStateService()
                );
        }

        public ChessGame BuildGame(IBoardSetup<ChessPieceEntity> setup, Colours start = Colours.White)
        {
            return new ChessGame(
                Logger<ChessGame>(),
                BuildEngineProvider(),
                BuildEntityFactory(),
                BuildGameStateService(),

                setup,
                start
            );
        }

    }
}