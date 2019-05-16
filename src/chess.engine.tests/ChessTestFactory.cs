using System;
using System.Collections.Generic;
using board.engine;
using board.engine.Actions;
using chess.engine.Chess;
using chess.engine.Chess.Actions;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Movement;
using chess.engine.Game;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace chess.engine.tests
{
    public static class ChessTestFactory
    {
        public enum LoggerType
        {
            Null, Injected, Mocked
        }
        public static Dictionary<Type, Mock> MockLoggers = new Dictionary<Type, Mock>();
        public static ILogger<T> Logger<T>(LoggerType type = LoggerType.Injected)
        {
            switch (type)
            {
                case LoggerType.Null:
                    return NullLogger<T>.Instance;
                case LoggerType.Injected:
                    return AppContainer.GetService<ILogger<T>>();
                case LoggerType.Mocked:
                    if (MockLoggers.TryGetValue(typeof(T), out var mock))
                    {
                        return (ILogger<T>) mock.Object;
                    }

                    throw new Exception($"Mock logger object of type {nameof(T)} not found in the the MockLoggers dictionary");
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        public static Mock<IBoardActionProvider<ChessPieceEntity>> BoardActionProviderMock(LoggerType logger = LoggerType.Null)
            => new Mock<IBoardActionProvider<ChessPieceEntity>>();
        public static Mock<IPlayerStateService> ChessGameStateServiceMock(LoggerType logger = LoggerType.Null)
            => new Mock<IPlayerStateService>();

        public static Mock<IBoardMoveService<ChessPieceEntity>> BoardMoveServiceMock(LoggerType logger = LoggerType.Null)
            => new Mock<IBoardMoveService<ChessPieceEntity>>();




//
//
//        public static ChessPieceEntityFactory ChessPieceEntityFactory(LoggerType logger = LoggerType.Injected)
//            => new ChessPieceEntityFactory();
//
//        public static ChessMoveValidationProvider MoveValidationProvider(LoggerType logger = LoggerType.Injected)
//            => new ChessMoveValidationProvider();
//
//        public static ChessPathValidator PathValidator(LoggerType logger = LoggerType.Injected)
//            => new ChessPathValidator(
//                Logger<ChessPathValidator>(logger),
//                MoveValidationProvider(logger));
//
//        public static ChessPathsValidator PathsValidator(LoggerType logger = LoggerType.Injected)
//            => new ChessPathsValidator(
//                Logger<ChessPathsValidator>(logger),
//                PathValidator(logger)
//                );
//
//
//        public static ChessRefreshAllPaths ChessRefreshAllPaths(LoggerType logger = LoggerType.Injected)
//            => new ChessRefreshAllPaths(
//                Logger<ChessRefreshAllPaths>(logger),
//                BoardActionProviderMock(logger),
//                PlayerStateService(logger),
//                CheckDetectionService(logger)
//                );
//
//        public static ICheckDetectionService CheckDetectionService(LoggerType logger = LoggerType.Injected)
//            => new CheckDetectionService(
//                Logger<CheckDetectionService>(),
//                BoardActionProviderMock(logger),
//                PlayerStateService(logger)
//                );
//
//        public static ChessGame NewChessGame(LoggerType logger = LoggerType.Injected)
//            => new ChessGame(
//                Logger<ChessGame>(logger),
//                ChessBoardEngineProvider(logger),
//                ChessPieceEntityFactory(logger),
//                PlayerStateService(logger));
//
//        public static ChessGame CustomChessGame(IBoardSetup<ChessPieceEntity> setup, Colours toPlay = Colours.White, LoggerType logger = LoggerType.Injected) 
//            => new ChessGame(
//                Logger<ChessGame>(logger),
//                ChessBoardEngineProvider(logger),
//                ChessPieceEntityFactory(logger),
//                PlayerStateService(logger),
//                setup,
//                toPlay
//                );
//
//
//        public static ChessBoardEngineProvider ChessBoardEngineProvider(LoggerType type = LoggerType.Injected) =>
//            new ChessBoardEngineProvider(
//                Logger<BoardEngine<ChessPieceEntity>>(),
//                ChessRefreshAllPaths(type),
//                PathsValidator(type),
//                BoardActionProviderMock(type)
//            );
    }
}