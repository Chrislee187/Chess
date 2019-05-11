using System.Linq;
using Chess.Common.Tests.Helpers;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using NUnit.Framework;

namespace Chess.Common.Tests.ValidMoveGeneration.Queen
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