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
        public void can_take_in_eight_directions()
        {
            // TODO: Will need to split this up once checks are implemented.
            const string asOneChar = "........" +
                                     "........" +
                                     "..rbr..." +
                                     "..nKn..." +
                                     "..ppp..." +
                                     "........" +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expectedTakes = BoardLocation.List("C6", "D6", "E6", "C5", "E5", "C4", "D4", "E4");

            var generator = new KingValidMoveGenerator();
            var chessMoves = generator.ValidMoves(board, "D5").ToList();

            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Take);
        }
    }
}