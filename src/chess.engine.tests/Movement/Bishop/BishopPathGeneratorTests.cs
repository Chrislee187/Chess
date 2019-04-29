using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Bishop;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Bishop
{
    public class BishopPathGeneratorTests : PathGeneratorTestsBase
    {
        private BishopPathGenerator _gen;

        [SetUp]
        public void SetUp()
        {
            _gen = new BishopPathGenerator();
        }

        [Test]
        public void PathsFrom_returns_all_directions()
        {
            var boardLocation = BoardLocation.At("E4");
            var whitePaths = _gen.PathsFrom(boardLocation, Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(4));

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("F5", ChessMoveType.MoveOrTake)
                    .To("G6", ChessMoveType.MoveOrTake)
                    .To("H7", ChessMoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("D5", ChessMoveType.MoveOrTake)
                    .To("C6", ChessMoveType.MoveOrTake)
                    .To("B7", ChessMoveType.MoveOrTake)
                    .To("A8", ChessMoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("F3", ChessMoveType.MoveOrTake)
                    .To("G2", ChessMoveType.MoveOrTake)
                    .To("H1", ChessMoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("D3", ChessMoveType.MoveOrTake)
                    .To("C2", ChessMoveType.MoveOrTake)
                    .To("B1", ChessMoveType.MoveOrTake)
                    .Build(), Colours.White);

        }
    }
}