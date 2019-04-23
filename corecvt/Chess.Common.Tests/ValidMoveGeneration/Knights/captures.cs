using System.Linq;
using Chess.Common.Tests.Helpers;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Chess.Common.Tests.ValidMoveGeneration.Knights
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
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var moves = _knightMoveGenerator.All(board, BoardLocation.At("B1")).Takes().ToList();

            AssertMovesContainsExpectedWithType(moves, expected, MoveType.Take);
        }
    }
}