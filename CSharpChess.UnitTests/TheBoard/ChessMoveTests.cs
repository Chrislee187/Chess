using NUnit.Framework;

namespace CSharpChess.UnitTests.TheBoard
{
    [TestFixture]
    public class ChessMoveTests
    {
        [TestCase("A2A4", ChessFile.A, 2, ChessFile.A, 4)]
        [TestCase("b7-b6", ChessFile.B, 7, ChessFile.B, 6)]
        public void can_explicitly_parse_move_strings(string move, ChessFile fromFile, int fromRank, ChessFile toFile, int toRank)
        {
            var chessMove = (Move) move;

            Assert.That(chessMove.From.File, Is.EqualTo(fromFile));
            Assert.That(chessMove.From.Rank, Is.EqualTo(fromRank));
            Assert.That(chessMove.To.File, Is.EqualTo(toFile));
            Assert.That(chessMove.To.Rank, Is.EqualTo(toRank));
        }

        [TestCase("A7A8Q", PieceNames.Queen)]
        [TestCase("A7-A8N", PieceNames.Knight)]
        [TestCase("A7A8=R", PieceNames.Rook)]
        [TestCase("A7-A8=b", PieceNames.Bishop)]
        public void can_explicitly_parse_move_strings_with_promotions(string move, PieceNames pieceName)
        {
            var chessMove = (Move)move;

            Assert.That(chessMove.From.File, Is.EqualTo(ChessFile.A));
            Assert.That(chessMove.From.Rank, Is.EqualTo(7));
            Assert.That(chessMove.To.File, Is.EqualTo(ChessFile.A));
            Assert.That(chessMove.To.Rank, Is.EqualTo(8));
            Assert.That(chessMove.MoveType, Is.EqualTo(MoveType.Promotion));
            Assert.That(chessMove.PromotedTo, Is.EqualTo(pieceName));
        }
    }
}