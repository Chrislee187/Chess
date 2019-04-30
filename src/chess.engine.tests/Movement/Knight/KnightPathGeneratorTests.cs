using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Knight;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Knight
{
    public class KnightPathGeneratorTests : PathGeneratorTestsBase
    {
        private KnightPathGenerator _gen;

        [SetUp]
        public void SetUp()
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
                new PathBuilder().From("E4").To("F6", ChessMoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("D6", ChessMoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("F2", ChessMoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("D2", ChessMoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("G5", ChessMoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("G3", ChessMoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("C5", ChessMoveType.MoveOrTake).Build(), Colours.White);
            AssertPathContains(whitePaths,
                new PathBuilder().From("E4").To("C3", ChessMoveType.MoveOrTake).Build(), Colours.White);
        }
    }

}