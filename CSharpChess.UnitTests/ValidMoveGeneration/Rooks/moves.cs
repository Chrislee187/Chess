using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Rooks
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        private RookValidMoveGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _generator = new RookValidMoveGenerator();
        }
        [Test]
        public void can_move_in_four_horizontal_directions()
        {
            const string asOneChar = "........" +
                                     ".P.P.P.." +
                                     "........" +
                                     ".P.R.P.." +
                                     "........" +
                                     ".P.P.P.." +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expected = BoardLocation.List("D6", "E5", "D4", "C5");

            var chessMoves = _generator.For(board, "D5");

            AssertMovesContainsExpectedWithType(chessMoves, expected, MoveType.Move);
        }
    }
}