using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Kings
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class covers : BoardAssertions
    {
        private KingMoveGenerator _kingMoveGenerator;

        [SetUp]
        public void SetUp()
        {
            _kingMoveGenerator = new KingMoveGenerator();
        }

        [TestCase("E1", new [] {"D1","D2","E2","F2","F1"})]
        [TestCase("E8", new[] { "D8", "D7", "E7", "F7", "F8" })]
        public void covers_surrounding_squares(string location, IEnumerable<string> expected)
        {
            var board = BoardBuilder.NewGame;

            var validMoves = _kingMoveGenerator.All(board, BoardLocation.At(location)).Covers();

            AssertMovesContainsExpectedWithType(validMoves, expected.Select(BoardLocation.At), MoveType.Cover);
        }
    }
}