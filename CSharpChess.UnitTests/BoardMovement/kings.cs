using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class kings : BoardAssertions
    {
        [Test]
        public void can_move_with_a_king()
        {
            const string asOneChar = "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "K.......";
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var result = board.Move("a1a2");

            AssertMoveSucceeded(result, board, "a1a2", new ChessPiece(Chess.Colours.White, Chess.PieceNames.King));
        }

        [Test]
        public void can_take_with_a_king()
        {
            const string asOneChar = "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "p......." +
                                     "K.......";
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var result = board.Move("a1a2");

            AssertTakeSucceeded(result, board, "a1a2", new ChessPiece(Chess.Colours.White, Chess.PieceNames.King));
        }


        [TestCase("E1C1", "A1D1")]
        [TestCase("E1G1", "H1F1")]
        [TestCase("E8C8", "A8D8")]
        [TestCase("E8G8", "H8F8")]
        public void can_castle(string kingMove, string expectedRookMove)
        {
            const string asOneChar = "r...k..r" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "R...K..R";

            var km = (ChessMove)kingMove;
            var colour = km.From.Rank == 1 ? Chess.Colours.White : Chess.Colours.Black;
            var board = BoardBuilder.CustomBoard(asOneChar, colour);

            var rm = (ChessMove)expectedRookMove;
            var moveResult = board.Move(km);

            Assert.That(moveResult.Succeeded, $"Failed: {kingMove}.");
            Assert.That(board[km.To].Piece.Is(colour, Chess.PieceNames.King));
            Assert.That(board[rm.To].Piece.Is(colour, Chess.PieceNames.Rook));
            Assert.That(board.IsEmptyAt(km.From));
            Assert.That(board.IsEmptyAt(rm.From));

        }
    }
}