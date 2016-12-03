using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Rooks
{
    [TestFixture]
    public class captures : BoardAssertions
    {
        private RookValidMoveGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _generator = new RookValidMoveGenerator();
        }
        [Test]
        public void can_take_in_four_diagonal_directions()
        {
            const string asOneChar = "........" +
                                     ".p.p.p.." +
                                     "........" +
                                     ".p.R.p.." +
                                     "........" +
                                     ".p.p.p.." +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expectedTakes = BoardLocation.List("F7", "F3", "B3", "B7");

            var chessMoves = _generator.For(board, "D5").ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }
    }
}