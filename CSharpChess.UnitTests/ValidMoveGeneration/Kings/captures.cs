using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Kings
{
    [TestFixture]
    public class captures : BoardAssertions
    {
        [Test]
        public void can_take()
        {
            // TODO: Will need to split this up once checks are implemented.
            const string asOneChar = "........" +
                                     "........" +
                                     "........" +
                                     "...K...." +
                                     "..ppp..." +
                                     "........" +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);
            var expectedTakes = BoardLocation.List("D4", "C4", "E4");

            var generator = new KingMoveGenerator();
            var chessMoves = generator.Takes(board, BoardLocation.At("D5")).ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }
    }
}