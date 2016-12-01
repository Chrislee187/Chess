using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration
{
    [TestFixture]
    public class PawnValidMovesTests
    {
        [TestCase("A2")]
        [TestCase("B2")]
        [TestCase("C2")]
        [TestCase("D2")]
        [TestCase("E2")]
        [TestCase("F2")]
        [TestCase("G2")]
        [TestCase("H2")]
        public void pawn_can_move_or_two_squares_forward_on_newboard(string pawnLocation)
        {
            var board = BoardBuilder.NewGame;

            var generateValidMoves = new PawnValidMoveGenerator();

            var from = BoardLocation.At(pawnLocation);
            var moves = generateValidMoves.For(board, from);

            var to1 = new BoardLocation(from.File, from.Rank + 1);
            var to2 = new BoardLocation(from.File, from.Rank + 2);

            moves.Single(m => m.To.Equals(to1));
            moves.Single(m => m.To.Equals(to2));

            Assert.That(moves.Count(), Is.EqualTo(2));
        }
    }
}