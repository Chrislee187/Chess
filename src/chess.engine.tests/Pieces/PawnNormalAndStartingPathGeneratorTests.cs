using System.Linq;
using chess.engine.Game;
using chess.engine.Movement;
using chess.engine.Pieces.Pawn;
using chess.engine.tests.Builders;
using NUnit.Framework;

namespace chess.engine.tests.Pieces
{
    public class PawnNormalAndStartingPathGeneratorTests : PathGeneratorTestsBase
    {
        private PawnNormalAndStartingPathGenerator _gen;

        [SetUp]
        public void SetUp()
        {
            _gen = new PawnNormalAndStartingPathGenerator();
        }

        [Test]
        public void PathsFrom_returns_both_starting_moves()
        {
            var boardLocation = BoardLocation.At("A2");
            var whitePaths = _gen.PathsFrom(boardLocation, Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(1));
            
            var ep = new PathBuilder().From("A2")
                .To("A3", ChessMoveType.MoveOnly)
                .To("A4", ChessMoveType.MoveOnly)
                .Build();

            AssertPathContains(whitePaths, ep, Colours.White);
        }

        [Test]
        public void PathsFrom_returns_single_move()
        {
            var startLocation = BoardLocation.At("A3");
            var whitePaths = _gen.PathsFrom(startLocation, Colours.White).ToList();
            Assert.That(whitePaths.Count(), Is.EqualTo(1));

            var ep = new PathBuilder().From(startLocation)
                .To("A4", ChessMoveType.MoveOnly)
                .Build();

            AssertPathContains(whitePaths, ep, Colours.White);
        }
    }
}