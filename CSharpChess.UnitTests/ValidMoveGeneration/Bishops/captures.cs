using System.Linq;
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
            const string asOneChar = "........" +
                                     ".p.p.p.." +
                                     "........" +
                                     ".p.B.p.." +
                                     "........" +
                                     ".p.p.p.." +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expectedTakes = BoardLocation.List("F7", "F3", "B7", "B3");

            var generator = new BishopValidMoveGenerator();
            var chessMoves = generator.ValidMoves(board, "D5").ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }

        [Test]
        public void can_cover_in_four_diagonal_directions()
        {
            const string asOneChar = "........" +
                                     ".P.p.P.." +
                                     "........" +
                                     ".p.B.p.." +
                                     "........" +
                                     ".P.p.P.." +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expectedTakes = BoardLocation.List("F7", "F3", "B7", "B3");

            var generator = new BishopValidMoveGenerator();
            var chessMoves = generator.Covers(board, BoardLocation.At("D5")).ToList();
            DumpBoardLocations(chessMoves.Select(x => x.To));
            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Cover);
        }

    }
}