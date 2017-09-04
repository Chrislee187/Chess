using System.Linq;
using CSharpChess.Rules;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
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

            var result = Pawns.EnPassantRules.Check(board, ChessMove.Create(pieceLoc, enemyPieceLoc)).All(rr => rr.Passed);

            Assert.True(result, "EnPassantRules check failed.");
        }

        [Test]
        public void cannot_enpassant_without_piece_to_take()
        {
            var board = BoardOneMoveBeforeEnPassantAvailable();

            var pieceLoc = BoardLocation.At("D4");
            var enemyPieceLoc = BoardLocation.At("C3");

            var result = Pawns.EnPassantRules.Check(board, ChessMove.Create(pieceLoc, enemyPieceLoc)).All(rr => rr.Passed);

            Assert.False(result, "EnPassantRules check failed.");
//            Assert.That(Chess.Rules.Failures[ChessMove.Create(pieceLoc, enemyPieceLoc)]);

        }


        private static ChessBoard BoardWithValidEnPassantAvailableForBlack()
        {
            var board = BoardOneMoveBeforeEnPassantAvailable();
            board.Move("c2c4");
            return board;
        }

        private static ChessBoard BoardOneMoveBeforeEnPassantAvailable()
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

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);
            return board;
        }
    }
}