using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.TheBoard
{
    [TestFixture]
    public class ChessMoveTests
    {
        [TestCase("A2A4", Chess.Board.ChessFile.A, 2, Chess.Board.ChessFile.A, 4)]
        [TestCase("b7-b6", Chess.Board.ChessFile.B, 7, Chess.Board.ChessFile.B, 6)]
        public void can_explicitly_parse_move_strings(string move, Chess.Board.ChessFile fromFile, int fromRank, Chess.Board.ChessFile toFile, int toRank)
        {
            var chessMove = (ChessMove) move;

            Assert.That(chessMove.From.File, Is.EqualTo(fromFile));
            Assert.That(chessMove.From.Rank, Is.EqualTo(fromRank));
            Assert.That(chessMove.To.File, Is.EqualTo(toFile));
            Assert.That(chessMove.To.Rank, Is.EqualTo(toRank));
        }

        [TestCase("A7A8Q", Chess.Board.PieceNames.Queen)]
        [TestCase("A7-A8N", Chess.Board.PieceNames.Knight)]
        [TestCase("A7A8=R", Chess.Board.PieceNames.Rook)]
        [TestCase("A7-A8=b", Chess.Board.PieceNames.Bishop)]
        public void can_explicitly_parse_move_strings_with_promotions(string move, Chess.Board.PieceNames pieceName)
        {
            var chessMove = (ChessMove)move;

            Assert.That(chessMove.From.File, Is.EqualTo(Chess.Board.ChessFile.A));
            Assert.That(chessMove.From.Rank, Is.EqualTo(7));
            Assert.That(chessMove.To.File, Is.EqualTo(Chess.Board.ChessFile.A));
            Assert.That(chessMove.To.Rank, Is.EqualTo(8));
            Assert.That(chessMove.MoveType, Is.EqualTo(MoveType.Promotion));
            Assert.That(chessMove.PromotedTo, Is.EqualTo(pieceName));
        }
    }
}