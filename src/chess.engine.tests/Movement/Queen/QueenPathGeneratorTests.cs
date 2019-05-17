using System.Linq;
using board.engine.Actions;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.ChessPieces.Queen;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Queen
{
    [TestFixture]
    public class QueenPathGeneratorTests : PathGeneratorTestsBase
    {
        private QueenPathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new QueenPathGenerator();
        }

        [Test]
        public void PathsFrom_returns_all_directions()
        {
            var boardLocation = "E4".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(boardLocation, (int) Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(8));

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("F5", (int) DefaultActions.MoveOrTake)
                    .To("G6", (int) DefaultActions.MoveOrTake)
                    .To("H7", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("D5", (int) DefaultActions.MoveOrTake)
                    .To("C6", (int) DefaultActions.MoveOrTake)
                    .To("B7", (int) DefaultActions.MoveOrTake)
                    .To("A8", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("F3", (int) DefaultActions.MoveOrTake)
                    .To("G2", (int) DefaultActions.MoveOrTake)
                    .To("H1", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("D3", (int) DefaultActions.MoveOrTake)
                    .To("C2", (int) DefaultActions.MoveOrTake)
                    .To("B1", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("F5", (int) DefaultActions.MoveOrTake)
                    .To("G6", (int) DefaultActions.MoveOrTake)
                    .To("H7", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("D5", (int) DefaultActions.MoveOrTake)
                    .To("C6", (int) DefaultActions.MoveOrTake)
                    .To("B7", (int) DefaultActions.MoveOrTake)
                    .To("A8", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("F3", (int) DefaultActions.MoveOrTake)
                    .To("G2", (int) DefaultActions.MoveOrTake)
                    .To("H1", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

            AssertPathContains(whitePaths,
                new ChessPathBuilder().From("E4")
                    .To("D3", (int) DefaultActions.MoveOrTake)
                    .To("C2", (int) DefaultActions.MoveOrTake)
                    .To("B1", (int) DefaultActions.MoveOrTake)
                    .Build(), Colours.White);

        }
    }
}