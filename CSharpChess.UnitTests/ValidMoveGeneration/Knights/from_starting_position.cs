using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Knights
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class from_starting_position : BoardAssertions
    {
        [TestCase("B1", new[] { "A3", "C3" })]
        [TestCase("G1", new[] { "F3", "H3" })]
        [TestCase("B8", new[] { "A6", "C6" })]
        [TestCase("G8", new[] { "F6", "H6" })]
        public void have_two_moves_at_start(string knightLocation, IEnumerable<string> expectedLocations)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = new KnightValidMoveGenerator().ValidMoves(board, knightLocation);

            AssertMovesContainsExpectedWithType(validMoves, expectedLocations.Select(l => BoardLocation.At(l)), MoveType.Move);
        }

        [Test]
        public void covers_pawns_to_its_front_sides()
        {
            var board = BoardBuilder.NewGame;
            var boardLocation = BoardLocation.At("B1");

            var covers = new KnightValidMoveGenerator().Covers(board, boardLocation).Select(m => m.To).ToList();

            Assert.That(covers, Contains.Item(BoardLocation.At("D2")));
        }
    }
}