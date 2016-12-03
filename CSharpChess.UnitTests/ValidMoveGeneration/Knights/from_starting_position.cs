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
    public class from_starting_position : BoardAssertions
    {
        [TestCase("B1", new[] { "A3", "C3" })]
        [TestCase("F1", new[] { "E3", "G3" })]
        [TestCase("B8", new[] { "A6", "C6" })]
        [TestCase("F8", new[] { "E6", "G6" })]
        public void have_two_moves_at_start(string knightLocation, IEnumerable<string> expectedLocations)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = new KnightValidMoveGenerator().For(board, knightLocation);

            AssertMovesAreAsExpected(validMoves, expectedLocations.Select(l => BoardLocation.At(l)));
        }

    }
}