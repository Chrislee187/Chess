using CSharpChess.UnitTests.BoardBuilderTests;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
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
        public void cannot_move_an_empty_square()
        {
            var board = BoardBuilder.NewGame;

            var result = board.Move("D6-D5");

            Assert.That(result.Succeeded, Is.False);
        }

    }
}