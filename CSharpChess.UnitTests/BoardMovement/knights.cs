using System.Linq;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class knights : BoardAssertions
    {
        [Test]
        public void can_move_at_start()
        {
            var board = BoardBuilder.NewGame;

            var result = board.Move("B1-C3");

            AssertMoveSucceeded(result, board, "B1-C3", PiecesFactory.White.Knight);

            var piece = board[result.Move.To];
            Assert.That(piece.MoveHistory.Count(), Is.EqualTo(1));
        }

        [Test]
        public void can_take()
        {
            string asOneChar;
            asOneChar = ".......k" +
                        "........" +
                        "....p..." +
                        "........" +
                        "...N...." +
                        "........" +
                        "........" +
                        ".......K";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("d4e6");

            AssertTakeSucceeded(result, board, "d4e6", new ChessPiece(Colours.White, PieceNames.Knight));
        }
    }
}