using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
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
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var moves = _knightMoveGenerator.All(board, BoardLocation.At("B1")).Covers().ToList();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Cover);
        }
    }
}