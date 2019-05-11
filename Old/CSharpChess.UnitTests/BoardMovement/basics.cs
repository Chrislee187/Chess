using System.Linq;
using CSharpChess.System;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class basics : BoardAssertions
    {
        [Test]
        public void black_cannot_move_when_whites_turn()
        {
            var board = BoardBuilder.NewGame;

            var result = board.Move("D7-D5");

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public void white_cannot_move_when_blacks_turn()
        {
            var board = BoardBuilder.CustomBoard(ChessBoardHelper.NewBoardInOneCharNotation, Colours.Black);

            var result = board.Move("D2-D4");

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
        public void fools_mate()
        {
            var board = BoardBuilder.NewGame;
            var moves = new[] {"f2f3", "e7e5", "g2g4", "d8h4"};

            moves.ToList().ForEach(m => board.Move(m));

            Assert.That(board.GameState, Is.EqualTo(GameState.CheckMateBlackWins));
        }
    }
}