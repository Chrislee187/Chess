using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Kings
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class from_starting_position : BoardAssertions
    {
        [TestCase("D1")]
        [TestCase("D8")]
        public void have_no_moves_at_start(string location)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = new KingMoveGenerator().Moves(board,BoardLocation.At(location));

            Assert.That(validMoves.Count(), Is.EqualTo(0));
        }

        [TestCase("D1")]
        [TestCase("D8")]
        public void have_no_takes_at_start(string location)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = new KingMoveGenerator().Takes(board, BoardLocation.At(location));

            Assert.That(validMoves.Count(), Is.EqualTo(0));
        }
    }
}