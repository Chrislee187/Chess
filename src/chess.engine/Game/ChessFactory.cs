using System;
using board.engine;
using board.engine.Movement;
using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Movement;
using chess.engine.Movement.King;
using chess.engine.SAN;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace chess.engine.Game
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

        public static ChessPieceEntityFactory ChessPieceEntityFactory(LoggerType logger = LoggerType.Injected)
            => new ChessPieceEntityFactory();

        public static IChessValidationSteps CastleValidationSteps()
            => new ChessValidationSteps();
        public static ChessMoveValidationProvider MoveValidationProvider(LoggerType logger = LoggerType.Injected)
            => new ChessMoveValidationProvider(CastleValidationSteps());

        public static ChessPathValidator PathValidator(
            IMoveValidationProvider<ChessPieceEntity> moveValidationProvider = null,
            LoggerType logger = LoggerType.Injected)
            => new ChessPathValidator(
                Logger<ChessPathValidator>(logger),
                moveValidationProvider ?? MoveValidationProvider(logger)
            );
//
//        private static ChessPathsValidator _pathsValidatorSingleton;
//        public static ChessPathsValidator PathsValidator(LoggerType logger = LoggerType.Injected,
//            IPathValidator<ChessPieceEntity> pathValidator = null
//            )
//        {
//            if (_pathsValidatorSingleton != null) return _pathsValidatorSingleton;
//
//            _pathsValidatorSingleton = new ChessPathsValidator(
//                Logger<ChessPathsValidator>(logger),
//                pathValidator ?? PathValidator(null, logger)
//            );
//            return _pathsValidatorSingleton;
//        }

        public static ChessPathsValidator PathsValidator(LoggerType logger = LoggerType.Injected,
            IPathValidator<ChessPieceEntity> pathValidator = null
        )
        {
            return new ChessPathsValidator(
                Logger<ChessPathsValidator>(logger),
                pathValidator ?? PathValidator(null, logger)
            );
        }
        public static ChessRefreshAllPaths ChessRefreshAllPaths(LoggerType logger = LoggerType.Injected,
            ChessBoardActionProvider chessBoardActionProvider = null
            )
            => new ChessRefreshAllPaths(
                Logger<ChessRefreshAllPaths>(logger),
                CheckDetectionService(logger)
            );

        public static IPlayerStateService PlayerStateService(LoggerType logger = LoggerType.Injected) 
        => new PlayerStateService(Logger<IPlayerStateService>(logger),
            FindAttackPaths(logger), PathsValidator(logger));

        public static IFindAttackPaths FindAttackPaths(LoggerType logger = LoggerType.Injected)
            => new FindAttackPaths();
        public static ChessBoardActionProvider ChessBoardActionProvider(IBoardEntityFactory<ChessPieceEntity> entityFactory = null, LoggerType logger = LoggerType.Injected)
            => new ChessBoardActionProvider(
                entityFactory ?? ChessPieceEntityFactory(logger)
                );

        public static ChessGame NewChessGame(LoggerType logger = LoggerType.Injected)
            => new ChessGame(
                Logger<ChessGame>(logger),
                ChessBoardEngineProvider(logger),
                ChessPieceEntityFactory(logger),
                CheckDetectionService(logger)
                );

        public static ChessGame CustomChessGame(IBoardSetup<ChessPieceEntity> setup, Colours toPlay = Colours.White, LoggerType logger = LoggerType.Injected) 
            => new ChessGame(
                Logger<ChessGame>(logger),
                ChessBoardEngineProvider(logger),
                CheckDetectionService(logger),
                setup,
                toPlay
                );

        public static IBoardMoveService<ChessPieceEntity> BoardMoveService(
            ChessBoardActionProvider boardActionProvider = null,
            IBoardEntityFactory<ChessPieceEntity> entityFactory = null,
            LoggerType logger = LoggerType.Injected
            )
        {
            return new BoardMoveService<ChessPieceEntity>(
                boardActionProvider ?? ChessBoardActionProvider(entityFactory, logger)
                );
        }

        public static ChessBoardEngineProvider ChessBoardEngineProvider(LoggerType logger = LoggerType.Injected) =>
            new ChessBoardEngineProvider(
                Logger<BoardEngine<ChessPieceEntity>>(logger),
                ChessRefreshAllPaths(logger),
                PathsValidator(logger),
                BoardMoveService(null, null, logger));

        public static ICheckDetectionService CheckDetectionService(LoggerType logger = LoggerType.Injected)
        {
            return new CheckDetectionService(
                Logger<CheckDetectionService>(logger),
                PlayerStateService(logger),
                BoardMoveService(null, null, logger),
                FindAttackPaths(logger),
                PathsValidator(logger)
            );
        }

        public static ISanTokenParser SanTokenFactory() => new SanTokenParser();

        public static ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData MoveExtraData(Colours owner,
            ChessPieceName piece)
            => new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData(owner, piece);
    }
}