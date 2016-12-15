using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Bishops
{
    [TestFixture]
    public class captures : BoardAssertions
    {
        [Test]
        public void can_take_in_four_diagonal_directions()
        {
            const string asOneChar = ".......k" +
                                     ".p.p.p.." +
                                     "........" +
                                     ".p.B.p.." +
                                     "........" +
                                     ".p.p.p.." +
                                     "........" +
                                     ".......K";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);
            var expectedTakes = BoardLocation.List("F7", "F3", "B7", "B3");

            var generator = new BishopMoveGenerator();
            var chessMoves = generator.All(board,BoardLocation.At("D5")).Takes().ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }


    }
}