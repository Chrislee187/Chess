using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.BoardBuilderTests;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Pawns
{
    [TestFixture]
    public class pawn_moves : BoardAssertions
    {
        [Test]
        public void pawn_is_promotable()
        {
            var asOneChar =
                "........" +
                "P......." +
                "........" +
                "........" +
                "........" +
                "........" +
                ".PPPPPPP" +
                "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var generator = new PawnValidMoveGenerator();
            var chessMoves = generator.For(board, "A7");

            Assert.That(chessMoves.Count(), Is.EqualTo(1));
            var chessMove = chessMoves.First();
            Assert.That(chessMove.From, Is.EqualTo("A7".ToBoardLocation()));
            Assert.That(chessMove.To, Is.EqualTo("A8".ToBoardLocation()));
            Assert.That(chessMove.PromotedTo, Is.EqualTo(Chess.PieceNames.Blank));
            Assert.That(chessMove.MoveType, Is.EqualTo(MoveType.Promotion));
        }
    }
}