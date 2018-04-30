using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.Rules
{
    [TestFixture]
    public class PawnTests
    {
        [Test]
        public void can_enpassant()
        {
            var board = BoardWithValidEnPassantAvailableForBlack();

            var pieceLoc = BoardLocation.At("D4");
            var enemyPieceLoc = BoardLocation.At("C3");

            var result = Pawns.EnPassantRules.Check(board, Move.Create(pieceLoc, enemyPieceLoc)).All(rr => rr.Passed);

            Assert.True(result, "EnPassantRules check failed.");
        }

        [Test]
        public void cannot_enpassant_without_piece_to_take()
        {
            var board = BoardOneMoveBeforeEnPassantAvailable();

            var pieceLoc = BoardLocation.At("D4");
            var enemyPieceLoc = BoardLocation.At("C3");

            var result = Pawns.EnPassantRules.Check(board, Move.Create(pieceLoc, enemyPieceLoc)).All(rr => rr.Passed);

            Assert.False(result, "EnPassantRules check failed.");
//            Assert.That(Info.Rules.Failures[Move.Create(pieceLoc, enemyPieceLoc)]);

        }


        private static Board BoardWithValidEnPassantAvailableForBlack()
        {
            var board = BoardOneMoveBeforeEnPassantAvailable();
            board.Move("c2c4");
            return board;
        }

        private static Board BoardOneMoveBeforeEnPassantAvailable()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "...p...." +
                "........" +
                "PPPPPPPP" +
                "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            return board;
        }
    }
}