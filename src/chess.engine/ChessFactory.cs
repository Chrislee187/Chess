using System;
using board.engine;
using chess.engine.Chess;
using chess.engine.Chess.Actions;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Movement;
using chess.engine.Game;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace chess.engine
{
    public static class ChessFactory
    {
        public enum LoggerType
        {
            Null, Injected
        }

        public static ILogger<T> Logger<T>(LoggerType type = LoggerType.Injected)
        {
            switch (type)
            {
                case LoggerType.Null:
                    return NullLogger<T>.Instance;
                case LoggerType.Injected:
                    return AppContainer.GetService<ILogger<T>>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static ChessPieceEntityProvider ChessPieceEntityProvider(LoggerType logger = LoggerType.Injected)
            => new ChessPieceEntityProvider();

        public static ChessMoveValidationProvider MoveValidationProvider(LoggerType logger = LoggerType.Injected)
            => new ChessMoveValidationProvider();

        public static ChessPathValidator PathValidator(LoggerType logger = LoggerType.Injected)
            => new ChessPathValidator(
                Logger<ChessPathValidator>(logger),
                MoveValidationProvider(logger));

        public static ChessPathsValidator PathsValidator(LoggerType logger = LoggerType.Injected)
            => new ChessPathsValidator(
                Logger<ChessPathsValidator>(logger),
                PathValidator(logger)
                );

        public static PlayerStateService ChessGameStateService(LoggerType logger = LoggerType.Injected)
            => new PlayerStateService(Logger<PlayerStateService>(logger));
        public static ChessRefreshAllPaths ChessRefreshAllPaths(LoggerType logger = LoggerType.Injected)
            => new ChessRefreshAllPaths(
                Logger<ChessRefreshAllPaths>(logger),
                ChessBoardActionProvider(logger),
                ChessGameStateService(logger),
                CheckDetectionService(logger)
                );

        public static ICheckDetectionService CheckDetectionService(LoggerType logger = LoggerType.Injected)
            => new CheckDetectionService(
                Logger<CheckDetectionService>(),
                ChessBoardActionProvider(logger),
                ChessGameStateService(logger)
                );

        public static IPlayerStateService PlayerStateService() 
        => new PlayerStateService(Logger<IPlayerStateService>());

        public static ChessBoardActionProvider ChessBoardActionProvider(LoggerType logger = LoggerType.Injected)
            => new ChessBoardActionProvider(ChessPieceEntityProvider(logger));


        public static ChessGame NewChessGame(LoggerType logger = LoggerType.Injected)
            => new ChessGame(
                Logger<ChessGame>(logger),
                ChessBoardEngineProvider(logger),
                ChessPieceEntityProvider(logger),
                ChessGameStateService(logger));

        public static ChessGame CustomChessGame(IBoardSetup<ChessPieceEntity> setup, Colours toPlay = Colours.White, LoggerType logger = LoggerType.Injected) 
            => new ChessGame(
                Logger<ChessGame>(logger),
                ChessBoardEngineProvider(logger),
                ChessPieceEntityProvider(logger),
                ChessGameStateService(logger),
                setup,
                toPlay
                );


        public static ChessBoardEngineProvider ChessBoardEngineProvider(LoggerType type = LoggerType.Injected) =>
            new ChessBoardEngineProvider(
                Logger<BoardEngine<ChessPieceEntity>>(),
                ChessRefreshAllPaths(type),
                PathsValidator(type),
                ChessBoardActionProvider(type)
            );
    }
}