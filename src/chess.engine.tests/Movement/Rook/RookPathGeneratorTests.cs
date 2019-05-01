using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.Rook;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Rook
{
    public class RookPathGeneratorTests : PathGeneratorTestsBase
    {
        private RookPathGenerator _gen;

        [SetUp]
        public void SetUp()
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
                    .To("E5", ChessMoveType.MoveOrTake)
                    .To("E6", ChessMoveType.MoveOrTake)
                    .To("E7", ChessMoveType.MoveOrTake)
                    .To("E8", ChessMoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("E3", ChessMoveType.MoveOrTake)
                    .To("E2", ChessMoveType.MoveOrTake)
                    .To("E1", ChessMoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("F4", ChessMoveType.MoveOrTake)
                    .To("G4", ChessMoveType.MoveOrTake)
                    .To("H4", ChessMoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("D4", ChessMoveType.MoveOrTake)
                    .To("C4", ChessMoveType.MoveOrTake)
                    .To("B4", ChessMoveType.MoveOrTake)
                    .To("A4", ChessMoveType.MoveOrTake)
                    .Build(), Colours.White);

        }
    }
}