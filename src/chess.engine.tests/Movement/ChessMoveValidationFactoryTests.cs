using System;
using board.engine.Movement;
using chess.engine.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class ChessMoveValidationFactoryTests
    {
        private ChessMoveValidationProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _provider = new ChessMoveValidationProvider();
        }
        [Test]
        public void FactorySupportsAllMoveTypes()
        {
            foreach (ChessMoveTypes type in Enum.GetValues(typeof(ChessMoveTypes)))
            {
                Assert.DoesNotThrow(() => _provider.Create((int)type, null), $"{type} is not support");
            }
        }
    }
}