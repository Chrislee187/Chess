using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Knights
{
    [TestFixture]
    public class covers : BoardAssertions
    {
        private KnightValidMoveGenerator _knightValidMoveGenerator;

        [SetUp]
        public void SetUp()
        {
            _knightValidMoveGenerator = new KnightValidMoveGenerator();
        }

        [Test]
        public void b1_covers_d2()
        {
            const string asOneChar = "rnbqkbnr" +
                                     "pppppppp" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "PPPPPPPP" +
                                     "RNBQKBNR";
            var expected = BoardLocation.List("d2");
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);

            var moves = _knightValidMoveGenerator.Covers(board, BoardLocation.At("B1")).ToList();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Cover);
        }
    }
}