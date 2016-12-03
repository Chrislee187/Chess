using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
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
    }
}