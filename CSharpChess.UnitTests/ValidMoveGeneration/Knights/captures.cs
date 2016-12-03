using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Knights
{
    [TestFixture]
    public class captures : BoardAssertions
    {
        private KnightValidMoveGenerator _knightValidMoveGenerator;

        [SetUp]
        public void SetUp()
        {
            _knightValidMoveGenerator = new KnightValidMoveGenerator();
        }

        [Test]
        public void can_capture()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "........" +
                "..p....." +
                "PPPPPPPP" +
                "RNBQKBNR";

            var expected = BoardLocation.List("A3", "C3");

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = _knightValidMoveGenerator.For(board, "B1").ToList();

            AssertMovesAreAsExpected(moves, expected);

            AssertMovesContains(moves, "A3", MoveType.Move);
            AssertMovesContains(moves, "C3", MoveType.Take);
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
                    "........"
                ;
            var expected = BoardLocation.List("E6", "F5", "F3", "E2", "C2", "B3", "B5", "C6");

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = _knightValidMoveGenerator.For(board, "D4").ToList();

            AssertMovesAreAsExpected(moves, expected);
            AssertAllMovesAreOfType(moves, MoveType.Move);
        }
    }
}