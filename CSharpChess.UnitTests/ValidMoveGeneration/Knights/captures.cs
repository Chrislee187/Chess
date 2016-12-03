using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Knights
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class captures : BoardAssertions
    {
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

            var moves = new KnightValidMoveGenerator().For(board, "B1");

            Assert.That(moves.First(m => m.To.Equals(BoardLocation.At("C3"))).MoveType
                , Is.EqualTo(MoveType.Take));
        }
   }
}