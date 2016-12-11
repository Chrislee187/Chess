using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Knights
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        private KnightMoveGenerator _knightMoveGenerator;

        [SetUp]
        public void SetUp()
        {
            _knightMoveGenerator = new KnightMoveGenerator();
        }
        [Test]
        public void all_moves_generated()
        {
            var asOneChar =
                    "........" +
                    "........" +
                    "........" +
                    "........" +
                    "...N...." +
                    "........" +
                    "........" +
                    "........";
            var expected = BoardLocation.List("E6", "F5", "F3", "E2", "C2", "B3", "B5", "C6");
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);

            var moves = _knightMoveGenerator.Moves(board, BoardLocation.At("D4")).ToList();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Move);
            AssertAllMovesAreOfType(moves, MoveType.Move);
        }
    }
}