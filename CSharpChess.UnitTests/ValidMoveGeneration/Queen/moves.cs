using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Queen
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        [Test]
        public void can_move_in_eight_directions()
        {
            const string asOneChar = "........" +
                                     ".P.P.P.." +
                                     "........" +
                                     ".P.Q.P.." +
                                     "........" +
                                     ".P.P.P.." +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);
            var expected = BoardLocation.List("E6", "E4", "C6", "C4", "D6", "E5", "D4", "C5");

            var generator = new QueenMoveGenerator();
            var chessMoves = generator.Moves(board, BoardLocation.At("D5"));

            AssertMovesContainsExpectedWithType(chessMoves, expected, MoveType.Move);
        }
    }
}