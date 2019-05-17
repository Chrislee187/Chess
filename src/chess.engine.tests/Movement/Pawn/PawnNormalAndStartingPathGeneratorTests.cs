using System.Linq;
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
        public void PathsFrom_returns_both_starting_moves()
        {
            var boardLocation = "A2".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(boardLocation, (int) Colours.White).ToList();

            Assert.That(whitePaths.Count(), Is.EqualTo(1));
            
            var ep = new ChessPathBuilder().From("A2")
                .To("A3")
                .To("A4")
                .Build();

            AssertPathContains(whitePaths, ep, Colours.White);
        }

        [Test]
        public void PathsFrom_returns_single_move()
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
        public void PathsFrom_returns_pawn_promotions()
        {
            var startLocation = "A7".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(startLocation, (int) Colours.White).ToList();
            Assert.That(whitePaths.Count(), Is.EqualTo(4));

            AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                .ToUpdatePiece("A8", ChessPieceName.Queen)
                .Build(), Colours.White);
            AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                .ToUpdatePiece("A8", ChessPieceName.Rook)
                .Build(), Colours.White);
            AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                .ToUpdatePiece("A8", ChessPieceName.Bishop)
                .Build(), Colours.White);
            AssertPathContains(whitePaths, new ChessPathBuilder().From(startLocation)
                .ToUpdatePiece("A8", ChessPieceName.Knight)
                .Build(), Colours.White);
        }
    }
}