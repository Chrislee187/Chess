using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
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
        public void can_take_in_four_directions()
        {
            //                        ABCDEFGH
            const string asOneChar = "........" + // 8
                                     "...p...." + // 7
                                     "........" + // 6
                                     ".p.R.p.." + // 5
                                     "........" + // 4
                                     "...p...." + // 3
                                     "........" + // 2
                                     "........";  // 1

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expectedTakes = BoardLocation.List("D7", "F5", "D3", "B5");

            var chessMoves = _generator.Takes(board,BoardLocation.At("D5")).ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }
    }
}