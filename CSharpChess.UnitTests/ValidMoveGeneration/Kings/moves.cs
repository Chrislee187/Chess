using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.MoveGeneration;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CSharpChess.UnitTests.ValidMoveGeneration.Kings
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        [Test]
        public void can_move_in_eight_directions()
        {
            const string asOneChar = ".......k" +
                                     "........" +
                                     "........" +
                                     "...K...." +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            var expected = BoardLocation.List("E6", "E4", "C6", "C4", "D6", "E5", "D4", "C5");

            var generator = new KingMoveGenerator();
            var chessMoves = generator.All(board, BoardLocation.At("D5")).Moves();

            AssertMovesContainsExpectedWithType(chessMoves, expected, MoveType.Move);
        }

        [TestCase("E1", new [] {"C1","G1"})]
        [TestCase("E8", new [] {"C8","G8"})]
        public void can_castle(string location, IEnumerable<string> expected )
        {
            const string asOneChar = "r...k..r" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "R...K..R";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var generator = new KingMoveGenerator();
            var chessMoves = generator.All(board, BoardLocation.At(location)).Moves();

            AssertMovesContains(chessMoves, expected, MoveType.Castle);
        }

    }
}