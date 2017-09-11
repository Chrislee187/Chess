using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Queen
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class from_starting_position : BoardAssertions
    {
        [TestCase("D1")]
        [TestCase("D8")]
        public void have_no_moves_at_start(string location)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = new QueenMoveGenerator().All(board, BoardLocation.At(location)).Moves();

            Assert.That(validMoves.Count(), Is.EqualTo(0));
        }
    }
}