using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Kings
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        [Test]
        public void can_move_in_eight_directions()
        {
            const string asOneChar = "........" +
                                     "........" +
                                     "........" +
                                     "...K...." +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expected = BoardLocation.List("E6", "E4", "C6", "C4", "D6", "E5", "D4", "C5");

            var generator = new KingValidMoveGenerator();
            var chessMoves = generator.Moves(board, BoardLocation.At("D5"));

            AssertMovesContainsExpectedWithType(chessMoves, expected, MoveType.Move);
        }

        [TestCase("E1", new [] {"C1","G1"})]
        [TestCase("E8", new [] {"C8","G8"})]
        public void can_castle(string location, IEnumerable<string> expected )
        {
            const string asOneChar = "r...k..r" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "R...K..R";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var generator = new KingValidMoveGenerator();
            var chessMoves = generator.Moves(board, BoardLocation.At(location));

            AssertMovesContains(chessMoves, expected, MoveType.Castle);
        }

        [Test]
        public void check_blocks_all_movement()
        {
            const string asOneChar = "...rkr.." +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "...n...." +
                                     "R...K..R";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var generator = new KingValidMoveGenerator();
            var chessMoves = generator.Moves(board, BoardLocation.At("E1"));

            Assert.True(chessMoves.Any(m => m.MoveType != MoveType.Castle));
            Assert.False(chessMoves.Any(m => m.To.ToString() == "D1"));
            Assert.False(chessMoves.Any(m => m.To.ToString() == "F1"));
        }
    }
}