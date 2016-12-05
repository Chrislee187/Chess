using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using CSharpChess.ValidMoves;
using NUnit.Framework;
// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Bishops
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        [Test]
        public void can_move_in_four_diagonal_directions()
        {
            const string asOneChar = "........" +
                                     ".P.P.P.." +
                                     "........" +
                                     ".P.B.P.." +
                                     "........" +
                                     ".P.P.P.." +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expected = BoardLocation.List("E6", "E4", "C6", "C4");

            var generator = new BishopValidMoveGenerator();
            var chessMoves = generator.ValidMoves(board, "D5");

            AssertMovesContainsExpectedWithType(chessMoves, expected, MoveType.Move);
        }
    }
}