using System;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Chess.Entities;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class MoveValidationFactoryTests
    {
        private MoveValidationProvider<ChessPieceEntity> _provider;

        [SetUp]
        public void SetUp()
        {
            _provider = new MoveValidationProvider<ChessPieceEntity>();
        }
        [Test]
        public void FactorySupportsAllMoveTypes()
        {
            foreach (ChessMoveTypes type in Enum.GetValues(typeof(DefaultActions)))
            {
                Assert.DoesNotThrow(() => _provider.Create((int)type, null), $"{type} is not support");
            }
        }
    }
}