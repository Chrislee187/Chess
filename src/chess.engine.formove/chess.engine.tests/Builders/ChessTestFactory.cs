using System;
using System.Collections.Generic;
using board.engine;
using board.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace chess.engine.tests.Builders
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
    }
}