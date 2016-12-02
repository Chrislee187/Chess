using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.BoardBuilderTests;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.Movement
{
    [TestFixture]
    public class basic_moves : BoardAssertions
    {
        [Test]
        public void black_cannot_move_when_whites_turn()
        {
            var board = BoardBuilder.NewGame;

            var result = board.Move("D7-D5");

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public void cannot_move_an_empty_square()
        {
            var board = BoardBuilder.NewGame;

            var result = board.Move("D6-D5");

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public void opening_pawn_move()
        {
            var board = BoardBuilder.NewGame;

            var result = board.Move("D2-D4");

            Assert.True(board.IsEmptyAt("d2"));
            Assert.True(board.IsNotEmptyAt("d4"));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.MoveType, Is.EqualTo(MoveType.Move));

            var piece = board[result.Move.To];
            Assert.That(piece.MoveHistory.Count(), Is.EqualTo(1));
            Assert.That(piece.MoveHistory.First().ToString(), Is.EqualTo("D2-D4"));
        }

    }
}