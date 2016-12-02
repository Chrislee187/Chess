using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.Movement
{
    [TestFixture]
    public class ChessMoveTests
    {
        [TestCase("A2A4", Chess.ChessFile.A, 2, Chess.ChessFile.A, 4)]
        [TestCase("b7-b6", Chess.ChessFile.B, 7, Chess.ChessFile.B, 6)]
        public void can_implicitly_parse_move_strings(string move, Chess.ChessFile fromFile, int fromRank, Chess.ChessFile toFile, int toRank)
        {
            var chessMove = (ChessMove) move;

            Assert.That(chessMove.From.File, Is.EqualTo(fromFile));
            Assert.That(chessMove.From.Rank, Is.EqualTo(fromRank));
            Assert.That(chessMove.To.File, Is.EqualTo(toFile));
            Assert.That(chessMove.To.Rank, Is.EqualTo(toRank));
        }

        [TestCase("A7A8Q", Chess.PieceNames.Queen)]
        [TestCase("A7-A8N", Chess.PieceNames.Knight)]
        [TestCase("A7A8=R", Chess.PieceNames.Rook)]
        [TestCase("A7-A8=b", Chess.PieceNames.Bishop)]
        public void can_implicitly_parse_move_strings_with_promotions(string move, Chess.PieceNames pieceName)
        {
            var chessMove = (ChessMove)move;

            Assert.That(chessMove.From.File, Is.EqualTo(Chess.ChessFile.A));
            Assert.That(chessMove.From.Rank, Is.EqualTo(7));
            Assert.That(chessMove.To.File, Is.EqualTo(Chess.ChessFile.A));
            Assert.That(chessMove.To.Rank, Is.EqualTo(8));
            Assert.That(chessMove.MoveType, Is.EqualTo(MoveType.Promotion));
            Assert.That(chessMove.PromotedTo, Is.EqualTo(pieceName));
        }
    }
}