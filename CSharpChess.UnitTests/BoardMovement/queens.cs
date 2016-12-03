using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class queens : BoardAssertions
    {
        [Test]
        public void can_move_with_a_queen()
        {
            const string asOneChar = "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "Q.......";
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var result = board.Move("a1h8");

            AssertMoveSucceeded(result, board, "a1h8", new ChessPiece(Chess.Colours.White, Chess.PieceNames.Queen));
        }

        [Test]
        public void can_take_with_a_queen()
        {
            const string asOneChar = ".......r" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "Q.......";
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var result = board.Move("a1h8");

            AssertTakeSucceeded(result, board, "a1h8", new ChessPiece(Chess.Colours.White, Chess.PieceNames.Queen));
        }
    }
}