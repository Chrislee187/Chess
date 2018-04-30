using CSharpChess.System;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class bishops : BoardAssertions
    {
        [Test]
        public void can_move_with_a_bishop()
        {
            const string asOneChar = "k......." +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "B......K";
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("a1h8");

            AssertMoveSucceeded(result, board, "a1h8", new ChessPiece(Colours.White, PieceNames.Bishop));
        }

        [Test]
        public void can_take_with_a_bishop()
        {
            const string asOneChar = "k......r" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "B.....K.";
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("a1h8");

            AssertTakeSucceeded(result, board, "a1h8", new ChessPiece(Colours.White, PieceNames.Bishop));
        }
    }
}