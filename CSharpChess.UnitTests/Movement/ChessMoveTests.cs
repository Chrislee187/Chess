using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.Movement
{
    [TestFixture]
    public class ChessMoveTests
    {
        [TestCase("A2-A4", Chess.ChessFile.A, 2, Chess.ChessFile.A, 4)]
        [TestCase("b7-b6", Chess.ChessFile.B, 7, Chess.ChessFile.B, 6)]
        public void can_implicitly_parse_move_strings(string move, Chess.ChessFile fromFile, int fromRank, Chess.ChessFile toFile, int toRank)
        {
            var chessMove = (ChessMove) move;

            Assert.That(chessMove.From.File, Is.EqualTo(fromFile));
            Assert.That(chessMove.From.Rank, Is.EqualTo(fromRank));
            Assert.That(chessMove.To.File, Is.EqualTo(toFile));
            Assert.That(chessMove.To.Rank, Is.EqualTo(toRank));
        }
    }
}