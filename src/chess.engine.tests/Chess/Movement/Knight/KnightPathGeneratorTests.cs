using System.Linq;
using chess.engine.Chess.Movement.ChessPieces.Knight;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.tests.Builders;
using chess.engine.tests.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Movement.Knight
{
    [TestFixture]
    public class KnightPathGeneratorTests : PathGeneratorTestsBase
    {
        private KnightPathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new KnightPathGenerator();
        }

        [Test]
        public void PathsFrom_returns_all_directions()
        {
            var boardLocation = BoardLocation.At("E4");
            var whitePaths = _gen.PathsFrom(boardLocation, Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(8));

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("F6", MoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("D6", MoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("F2", MoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("D2", MoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("G5", MoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("G3", MoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("C5", MoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("C3", MoveType.MoveOrTake).Build(), Colours.White);
        }
    }

}