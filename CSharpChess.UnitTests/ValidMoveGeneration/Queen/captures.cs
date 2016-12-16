using System.Linq;
using CSharpChess.MoveGeneration;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
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
            const string asOneChar = ".......k" +
                                     ".p.p.p.." +
                                     "........" +
                                     ".p.Q.p.." +
                                     "........" +
                                     ".p.p.p.." +
                                     "........" +
                                     ".......K";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expectedTakes = BoardLocation.List("F7", "F3", "B7", "B3", "D7", "F5", "D3", "B5");

            var generator = new QueenMoveGenerator();
            var chessMoves = generator.All(board, BoardLocation.At("D5")).Takes().ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }
    }
}