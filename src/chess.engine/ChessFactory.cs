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

        public static ILogger<T> CreateLogger<T>(LoggerType type = LoggerType.Injected)
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

        public static ChessPieceEntityFactory ChessPieceEntityFactory(LoggerType logger = LoggerType.Injected)
            => new ChessPieceEntityFactory();

        public static ChessMoveValidationProvider MoveValidationFactory(LoggerType logger = LoggerType.Injected)
            => new ChessMoveValidationProvider();

        public static ChessPathValidator PathValidator(LoggerType logger = LoggerType.Injected)
            => new ChessPathValidator(
                CreateLogger<ChessPathValidator>(logger),
                MoveValidationFactory(logger));

        public static ChessPathsValidator PathsValidator(LoggerType logger = LoggerType.Injected)
            => new ChessPathsValidator(
                CreateLogger<ChessPathsValidator>(logger),
                PathValidator(logger)
                );

        public static ChessGameStateService ChessGameStateService(LoggerType logger = LoggerType.Injected)
            => new ChessGameStateService(CreateLogger<ChessGameStateService>(logger));
        public static ChessRefreshAllPaths RefreshAllPaths(LoggerType logger = LoggerType.Injected)
            => new ChessRefreshAllPaths(
                CreateLogger<ChessRefreshAllPaths>(logger),
                ChessBoardActionProvider(logger),
                ChessGameStateService(logger)
                );

        public static ChessBoardActionProvider ChessBoardActionProvider(LoggerType logger = LoggerType.Injected)
            => new ChessBoardActionProvider(ChessPieceEntityFactory(logger));


        public static ChessGame NewChessGame(LoggerType logger = LoggerType.Injected)
            => new ChessGame(
                CreateLogger<ChessGame>(logger),
                ChessBoardEngineProvider(logger),
                ChessPieceEntityFactory(logger),
                ChessGameStateService(logger));

        public static ChessGame CustomChessGame(IBoardSetup<ChessPieceEntity> setup, Colours toPlay = Colours.White, LoggerType logger = LoggerType.Injected) 
            => new ChessGame(
                CreateLogger<ChessGame>(logger),
                ChessBoardEngineProvider(logger),
                ChessPieceEntityFactory(logger),
                ChessGameStateService(logger),
                setup,
                toPlay
                );


        public static ChessBoardEngineProvider ChessBoardEngineProvider(LoggerType type = LoggerType.Injected) =>
            new ChessBoardEngineProvider(
                CreateLogger<BoardEngine<ChessPieceEntity>>(),
                RefreshAllPaths(type),
                PathsValidator(type),
                ChessBoardActionProvider(type)
            );
    }
}