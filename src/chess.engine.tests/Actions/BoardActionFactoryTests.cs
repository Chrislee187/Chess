using System;
using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Actions
{
    [TestFixture]
    public class BoardActionFactoryTests
    {
        private BoardActionFactory<ChessPieceEntity> _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new BoardActionFactory<ChessPieceEntity>();
        }
        [Test]
        public void FactorySupportsAllMoveTypes()
        {
            foreach (MoveType type in Enum.GetValues(typeof(MoveType)))
            {
                Assert.DoesNotThrow(() => _factory.Create(type, null), $"{type} is not support");
            }
        }
    }
}