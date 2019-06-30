using System.Linq;
using board.engine.Actions;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.Rook;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Rook
{
    [TestFixture]
    public class RookPathGeneratorTests : ChessPathGeneratorTestsBase
    {
        private RookPathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new RookPathGenerator();
        }

        [Test]
        public void PathsFrom_generates_all_directions()
        {
            var boardLocation = "E4".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(boardLocation, (int) Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(4));

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("E5", (int) DefaultActions.MoveOrTake)
                    .To("E6", (int) DefaultActions.MoveOrTake)
                    .To("E7", (int) DefaultActions.MoveOrTake)
                    .To("E8", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("E3", (int) DefaultActions.MoveOrTake)
                    .To("E2", (int) DefaultActions.MoveOrTake)
                    .To("E1", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("F4", (int) DefaultActions.MoveOrTake)
                    .To("G4", (int) DefaultActions.MoveOrTake)
                    .To("H4", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("D4", (int) DefaultActions.MoveOrTake)
                    .To("C4", (int) DefaultActions.MoveOrTake)
                    .To("B4", (int) DefaultActions.MoveOrTake)
                    .To("A4", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

        }
    }
}