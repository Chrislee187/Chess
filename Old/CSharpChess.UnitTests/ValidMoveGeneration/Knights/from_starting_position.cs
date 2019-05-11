using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.UnitTests.Helpers;
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

            var moves = new KnightMoveGenerator().All(board,BoardLocation.At(knightLocation)).Moves().ToList();

            AssertMovesContainsExpectedWithType(moves, expectedLocations.Select(l => BoardLocation.At(l)), MoveType.Move);
        }

        [TestCase("B1", new[] { "D2" })]
        [TestCase("G1", new[] { "E2" })]
        [TestCase("B8", new[] { "D7" })]
        [TestCase("G8", new[] { "E7" })]
        public void covers_pawns_to_its_inner_sides(string knightLocation, IEnumerable<string> expectedLocations)
        {
            var board = BoardBuilder.NewGame;
            var boardLocation = BoardLocation.At(knightLocation);

            var covers = new KnightMoveGenerator().All(board, boardLocation).Covers().ToList();

            AssertMovesContainsExpectedWithType(covers, expectedLocations.Select(l => BoardLocation.At(l)), MoveType.Cover);
        }

        [TestCase("B1")]
        [TestCase("G1")]
        [TestCase("B8")]
        [TestCase("G8")]
        public void has_no_takes(string knightLocation)
        {
            var board = BoardBuilder.NewGame;
            var boardLocation = BoardLocation.At(knightLocation);

            var covers = new KnightMoveGenerator().All(board, boardLocation).Takes().ToList();

            Assert.That(covers, Is.Empty);
        }

    }
}