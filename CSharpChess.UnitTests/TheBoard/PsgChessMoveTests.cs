using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace CSharpChess.UnitTests.TheBoard
{
    [TestFixture]
    public class PgnChessMoveTests
    {
        [Test, Explicit("WIP")]
        public void can_parse_opening_pawn_move()
        {
            var expected = (ChessMove) "e2-e4";
            var pgnMove = "e4";
            var board = BoardBuilder.NewGame;

            var chessMove = PgnChessMove.Parse(pgnMove, board);

            Assert.That(chessMove.From.Rank, Is.EqualTo(expected.From.Rank));
            Assert.That(chessMove.From.File, Is.EqualTo(expected.From.File));
            Assert.That(chessMove.MoveType, Is.EqualTo(MoveType.Move));
        }
    }

}