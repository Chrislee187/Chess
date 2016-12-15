using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class rooks : BoardAssertions
    {
        private ChessPiece PieceUnderTest = new ChessPiece(Chess.Board.Colours.White, Chess.Board.PieceNames.Rook);

        [Test]
        public void can_move_with_a_rook()
        {
            const string asOneChar = "..k....." +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     ".......K" +
                                     "R.......";
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);

            var result = board.Move("a1a8");

            AssertMoveSucceeded(result, board, "a1a8", PieceUnderTest);
        }

        [Test]
        public void can_take_with_a_rook()
        {
            const string asOneChar = "r......." +
                                     ".......k" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "R......K";
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);

            var result = board.Move("a1a8");

            AssertTakeSucceeded(result, board, "a1A8", PieceUnderTest);
        }
    }
}