using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.MoveGeneration;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Rooks
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class from_starting_position : BoardAssertions
    {
        private RookMoveGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _generator = new RookMoveGenerator();
        }
        [TestCase("A1")]
        [TestCase("H1")]
        [TestCase("A8")]
        [TestCase("H8")]
        public void have_no_moves_at_start(string location)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = _generator.All(board, BoardLocation.At(location)).Moves();

            Assert.That(validMoves.Count(), Is.EqualTo(0));
        }
    }
}