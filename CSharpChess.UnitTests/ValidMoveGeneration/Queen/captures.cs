using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Queen
{
    [TestFixture]
    public class captures : BoardAssertions
    {
        [Test]
        public void can_take_in_eight_directions()
        {
            const string asOneChar = "........" +
                                     ".p.p.p.." +
                                     "........" +
                                     ".p.Q.p.." +
                                     "........" +
                                     ".p.p.p.." +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expectedTakes = BoardLocation.List("F7", "F3", "B7", "B3", "D7", "F5", "D3", "B5");

            var generator = new QueenValidMoveGenerator();
            var chessMoves = generator.For(board, "D5").ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }
    }
}