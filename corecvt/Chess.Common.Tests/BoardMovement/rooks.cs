using Chess.Common.Tests.Helpers;
using CSharpChess;
using CSharpChess.System;
using NUnit.Framework;

namespace Chess.Common.Tests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class rooks : BoardAssertions
    {
        private readonly ChessPiece _pieceUnderTest = new ChessPiece(Colours.White, PieceNames.Rook);

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
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("a1a8");

            AssertMoveSucceeded(result, board, "a1a8", _pieceUnderTest);
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
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("a1a8");

            AssertTakeSucceeded(result, board, "a1A8", _pieceUnderTest);
        }
    }
}