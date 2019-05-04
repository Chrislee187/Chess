using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.ChessPieces.Rook;
using chess.engine.tests.Builders;
using chess.engine.tests.Movement;
using NUnit.Framework;

namespace chess.engine.tests.Chess.Movement.Rook
{
    [TestFixture]
    public class RookPathGeneratorTests : PathGeneratorTestsBase
    {
        private RookPathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new RookPathGenerator();
        }

        [Test]
        public void PathsFrom_returns_all_directions()
        {
            var boardLocation = BoardLocation.At("E4");
            var whitePaths = _gen.PathsFrom(boardLocation, Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(4));

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("E5", MoveType.MoveOrTake)
                    .To("E6", MoveType.MoveOrTake)
                    .To("E7", MoveType.MoveOrTake)
                    .To("E8", MoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("E3", MoveType.MoveOrTake)
                    .To("E2", MoveType.MoveOrTake)
                    .To("E1", MoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("F4", MoveType.MoveOrTake)
                    .To("G4", MoveType.MoveOrTake)
                    .To("H4", MoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("D4", MoveType.MoveOrTake)
                    .To("C4", MoveType.MoveOrTake)
                    .To("B4", MoveType.MoveOrTake)
                    .To("A4", MoveType.MoveOrTake)
                    .Build(), Colours.White);

        }
    }
}