using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Bishops
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public partial class from_starting_position : BoardAssertions
    {
        [TestCase("B1")]
        [TestCase("G1")]
        [TestCase("B8")]
        [TestCase("G8")]
        public void have_no_moves_at_start(string knightLocation)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = new BishopValidMoveGenerator().ValidMoves(board, knightLocation);

            Assert.That(validMoves.Count(), Is.EqualTo(0));
        }

        [Test]
        public void covers_pawns_to_its_front_sides()
        {
            var board = BoardBuilder.NewGame;
            var boardLocation = BoardLocation.At("C1");

            var covers = new BishopValidMoveGenerator().Covers(board, boardLocation).Select(m => m.To).ToList();

            Assert.That(covers, Contains.Item(BoardLocation.At("B2")));
            Assert.That(covers, Contains.Item(BoardLocation.At("D2")));
        }
    }
}