using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
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

            Assert.True(result.Succeeded);
            Assert.True(board.IsEmptyAt("b1"), "Knight start square not empty.");
            Assert.True(board.IsNotEmptyAt("c3"), "Knight destination square empty.");
            Assert.True(board["c3"].Piece.Is(Chess.Colours.White, Chess.PieceNames.Knight), "Knight not found at destination");

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.MoveType, Is.EqualTo(MoveType.Move));

            var piece = board[result.Move.To];
            Assert.That(piece.MoveHistory.Count(), Is.EqualTo(1));
        }
    }
}