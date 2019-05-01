using System;
using chess.engine.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class MoveValidationFactoryTests
    {
        private MoveValidationFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new MoveValidationFactory();
        }
        [Test]
        public void FactorySupportsAllMoveTypes()
        {
            foreach (ChessMoveType type in Enum.GetValues(typeof(ChessMoveType)))
            {
                Assert.DoesNotThrow(() => _factory.Create(type, null), $"{type} is not support");
            }
        }
    }
}