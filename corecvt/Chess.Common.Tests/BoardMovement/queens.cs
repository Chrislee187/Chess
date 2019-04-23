using Chess.Common.Tests.Helpers;
using CSharpChess;
using CSharpChess.System;
using NUnit.Framework;

namespace Chess.Common.Tests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class queens : BoardAssertions
    {
        [Test]
        public void can_move_with_a_queen()
        {
            const string asOneChar = "........" +
                                     ".k......" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "Q......K";
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("a1h8");

            AssertMoveSucceeded(result, board, "a1h8", new ChessPiece(Colours.White, PieceNames.Queen));
        }

        [Test]
        public void can_take_with_a_queen()
        {
            const string asOneChar = ".......r" +
                                     ".k......" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "Q.....K.";
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("a1h8");

            AssertTakeSucceeded(result, board, "a1h8", new ChessPiece(Colours.White, PieceNames.Queen));
        }
    }
}