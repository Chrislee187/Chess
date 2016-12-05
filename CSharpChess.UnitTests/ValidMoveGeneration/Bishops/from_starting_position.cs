using System.Linq;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Bishops
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public partial class from_starting_position : BoardAssertions
    {
        [TestCase("C1")]
        [TestCase("F1")]
        [TestCase("C8")]
        [TestCase("F8")]
        public void have_no_moves_at_start(string knightLocation)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = new KnightValidMoveGenerator().ValidMoves(board, knightLocation);

            Assert.That(validMoves.Count(), Is.EqualTo(0));
        }
    }
}