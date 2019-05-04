using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Movement.ChessPieces.Bishop;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Bishop
{
    [TestFixture]
    public class BishopPathGeneratorTests : PathGeneratorTestsBase
    {
        private BishopPathGenerator _gen;

        [SetUp]
        public new void SetUp()
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
                    .To("F5", MoveType.MoveOrTake)
                    .To("G6", MoveType.MoveOrTake)
                    .To("H7", MoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("D5", MoveType.MoveOrTake)
                    .To("C6", MoveType.MoveOrTake)
                    .To("B7", MoveType.MoveOrTake)
                    .To("A8", MoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("F3", MoveType.MoveOrTake)
                    .To("G2", MoveType.MoveOrTake)
                    .To("H1", MoveType.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new PathBuilder().From("E4")
                    .To("D3", MoveType.MoveOrTake)
                    .To("C2", MoveType.MoveOrTake)
                    .To("B1", MoveType.MoveOrTake)
                    .Build(), Colours.White);

        }
    }
}