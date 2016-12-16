using System.Linq;
using CSharpChess.MoveGeneration;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Knights
{
    [TestFixture]
    public class covers : BoardAssertions
    {
        private KnightMoveGenerator _knightMoveGenerator;

        [SetUp]
        public void SetUp()
        {
            _knightMoveGenerator = new KnightMoveGenerator();
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
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = _knightMoveGenerator.All(board, BoardLocation.At("B1")).Covers().ToList();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Cover);
        }
    }
}