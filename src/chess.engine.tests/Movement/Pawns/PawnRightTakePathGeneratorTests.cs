using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Pawn;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Movement.Pawns
{
    public class PawnRightTakePathGeneratorTests : PathGeneratorTestsBase
    {
        private PawnRightTakePathGenerator _gen;

        [SetUp]
        public void SetUp()
        {
            _gen = new PawnRightTakePathGenerator();
        }
        [Test]
        public void PathsFrom_returns_empty_list_when_on_right_edge()
        {

            Assert.That(_gen.PathsFrom(BoardLocation.At("H2"), Colours.White).Count(), Is.EqualTo(0));
            Assert.That(_gen.PathsFrom(BoardLocation.At("A7"), Colours.Black).Count(), Is.EqualTo(0));
        }

        [Test]
        public void PathsFrom_returns_return_take()
        {
            var pieceLocation = BoardLocation.At("B2");
            var paths = _gen.PathsFrom(pieceLocation, Colours.White).ToList();

            var ep = new PathBuilder().From(pieceLocation)
                .To("C3", ChessMoveType.TakeOnly)
                .Build();

            AssertPathContains(paths, ep, Colours.White);
            Assert.That(paths.Count(), Is.EqualTo(1));
        }
    }
}