using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.MoveGeneration;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Knights
{
    [TestFixture]
    public class captures : BoardAssertions
    {
        private KnightMoveGenerator _knightMoveGenerator;

        [SetUp]
        public void SetUp()
        {
            _knightMoveGenerator = new KnightMoveGenerator();
        }

        [Test]
        public void can_capture()
        {
            const string asOneChar = "rnbqkbnr" +
                                     "pppppppp" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "..p....." +
                                     "PPPPPPPP" +
                                     "RNBQKBNR";
            var expected = BoardLocation.List("C3");
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);

            var moves = _knightMoveGenerator.All(board, BoardLocation.At("B1")).Takes().ToList();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Take);
        }
    }
}