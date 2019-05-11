using Chess.Common.Tests.Helpers;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Chess.Common.Tests.ValidMoveGeneration.Bishops
{
    [TestFixture]
    public class moves : BoardAssertions
    {
        [Test]
        public void can_move_in_four_diagonal_directions()
        {
            const string asOneChar = ".......k" +
                                     ".p.P.P.." +
                                     "........" +
                                     ".P.B.P.." +
                                     "........" +
                                     ".P.P.P.." +
                                     "........" +
                                     ".......K";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            var expected = BoardLocation.List("E6", "E4", "C6", "C4");

            var generator = new BishopMoveGenerator();
            var chessMoves = generator.All(board,BoardLocation.At("D5")).Moves();

            AssertMovesContainsExpectedWithType(chessMoves, expected, MoveType.Move);
        }

        [Test]
        public void cannot_uncover_check()
        {
            const string asOneChar = ".......k" +
                                     "........" +
                                     "........" +
                                     "...r...." +
                                     "........" +
                                     "........" +
                                     "...B...." +
                                     "...K....";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var at = BoardLocation.At("D2");

            var chessMoves = board.RemoveMovesThatLeaveBoardInCheck(at).ToList();
            Assert.True(chessMoves.None(), chessMoves.ToCSV());
        }
    }
}