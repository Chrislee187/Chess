using System;
using System.Linq;
using board.engine.Actions;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.Pawn;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Pawn
{
    [TestFixture]
    public class PawnTakePathGeneratorTests : ChessPathGeneratorTestsBase
    {
        // TODO: These are the old right path gen tests, more & better tests needed
        private PawnTakePathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new PawnTakePathGenerator();
        }
        [Test]
        public void PathsFrom_generates_empty_list_when_on_right_edge()
        {

            Assert.That(_gen.PathsFrom("H2".ToBoardLocation(), (int)Colours.White).Count(), Is.EqualTo(1));
            Assert.That(_gen.PathsFrom("A7".ToBoardLocation(), (int)Colours.Black).Count(), Is.EqualTo(1));
        }

        [Test]
        public void PathsFrom_generates_take()
        {
            var pieceLocation = "B2".ToBoardLocation();
            var paths = _gen.PathsFrom(pieceLocation, (int)Colours.White).ToList();

            var ep = new ChessPathBuilder().From(pieceLocation)
                .To("C3", (int)DefaultActions.TakeOnly)
                .Build();

            AssertPathContains(paths, ep, Colours.White);
            Assert.That(paths.Count(), Is.EqualTo(2));
        }


        [Test]
        public void PathsFrom_generates_all_pawn_promotions()
        {
            var startLocation = "B7".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(startLocation, (int)Colours.White).ToList();
            Assert.That(whitePaths.Count(), Is.EqualTo(8));

            foreach (var chessPieceName in new[]{ChessPieceName.Knight, ChessPieceName.Bishop, ChessPieceName.Rook, ChessPieceName.Queen})
            {
                AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                    .ToUpdatePiece("A8", chessPieceName, DefaultActions.UpdatePieceWithTake)
                    .Build(), Colours.White);

                AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                    .ToUpdatePiece("C8", chessPieceName, DefaultActions.UpdatePieceWithTake)
                    .Build(), Colours.White);
            }
        }
    }
}