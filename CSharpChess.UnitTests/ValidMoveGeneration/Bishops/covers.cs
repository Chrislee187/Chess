using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Bishops
{
    [TestFixture]
    public class covers : BoardAssertions
    {
        [Test]
        public void can_cover_in_four_diagonal_directions()
        {
            const string asOneChar = ".......k" +
                                     ".P.p.P.." +
                                     "........" +
                                     ".p.B.p.." +
                                     "........" +
                                     ".P.p.P.." +
                                     "........" +
                                     ".......K";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            var expectedTakes = BoardLocation.List("F7", "F3", "B7", "B3");

            var generator = new BishopMoveGenerator();
            var chessMoves = generator.All(board, BoardLocation.At("D5")).Covers().ToList();
            AssertMovesContainsExpectedWithType(chessMoves, expectedTakes, MoveType.Cover);
        }

    }
}