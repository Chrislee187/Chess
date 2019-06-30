using System.Linq;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.Pawn;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Pawn
{
    [TestFixture]
    public class PawnNormalAndStartingPathGeneratorTests : ChessPathGeneratorTestsBase
    {
        private PawnNormalAndStartingPathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new PawnNormalAndStartingPathGenerator();
        }

        [Test]
        public void PathsFrom_generates_both_starting_moves()
        {
            var boardLocation = "A2".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(boardLocation, (int) Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(1));
            
            var ep = new ChessPathBuilder().From("A2")
                .To("A3")
                .To("A4", (int) ChessMoveTypes.PawnTwoStep)
                .Build();

            AssertPathContains(whitePaths, ep, Colours.White);
        }

        [Test]
        public void PathsFrom_generates_single_move()
        {
            var startLocation = "A3".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(startLocation, (int)Colours.White).ToList();
            Assert.That(whitePaths.Count(), Is.EqualTo(1));

            var ep = new ChessPathBuilder().From(startLocation)
                .To("A4")
                .Build();
            AssertPathContains(whitePaths, ep, Colours.White);
        }


        [Test]
        public void PathsFrom_generates_all_pawn_promotions()
        {
            var startLocation = "A7".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(startLocation, (int) Colours.White).ToList();
            Assert.That(whitePaths.Count(), Is.EqualTo(4));


            foreach (var chessPieceName in new[] { ChessPieceName.Knight, ChessPieceName.Bishop, ChessPieceName.Rook, ChessPieceName.Queen })
            {
                AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                    .ToUpdatePiece("A8", chessPieceName)
                    .Build(), Colours.White);
            }
        }
    }
}