using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.MoveGeneration;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Bishops
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class from_starting_position : BoardAssertions
    {
        private BishopMoveGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _generator = new BishopMoveGenerator();
        }

        [TestCase("B1")]
        [TestCase("G1")]
        [TestCase("B8")]
        [TestCase("G8")]
        public void have_no_moves_at_start(string knightLocation)
        {
            var board = BoardBuilder.NewGame;

            var moves = _generator.All(board, BoardLocation.At(knightLocation)).Moves();

            Assert.That(moves, Is.Empty);
        }

        [TestCase("C1", new [] {"B2","D2"})]
        [TestCase("F1", new [] {"E2","G2"})]
        [TestCase("C8", new [] {"B7","D7"})]
        [TestCase("G8", new[] { "E7", "G7" })]
        public void covers_pawns_to_its_front_sides(string location, IEnumerable<string> expected )
        {
            var board = BoardBuilder.NewGame;
            var boardLocation = BoardLocation.At("C1");

            var covers = _generator.All(board, boardLocation).Covers().Select(m => m.To).ToList();

            Assert.That(covers, Contains.Item(BoardLocation.At("B2")));
            Assert.That(covers, Contains.Item(BoardLocation.At("D2")));
        }

        [TestCase("B1")]
        [TestCase("G1")]
        [TestCase("B8")]
        [TestCase("G8")]
        public void have_no_takes_at_start(string location)
        {
            var board = BoardBuilder.NewGame;

            var moves = _generator.All(board, BoardLocation.At(location)).Takes();

            Assert.That(moves, Is.Empty);
        }
    }
}