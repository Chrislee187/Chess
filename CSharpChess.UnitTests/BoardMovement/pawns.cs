using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class pawns : BoardAssertions
    {
        [Test]
        public void can_move_two_forward_at_start()
        {
            var board = BoardBuilder.NewGame;

            var result = board.Move("D2-D4");

            AssertMoveSucceeded(result, board, "D2-D4", Chess.Pieces.White.Pawn);

            var piece = board[result.Move.To];
            Assert.That(piece.MoveHistory.Count(), Is.EqualTo(1));
            Assert.That(piece.MoveHistory.First().ToString(), Is.EqualTo("D2-D4"));
        }

        [Test]
        public void can_take_enpassant()
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

            board.Move("c2c4");
            var result = board.Move("d4c3");

            Assert.That(result.Succeeded, "Enpassant move failed");
            Assert.That(result.MoveType, Is.EqualTo(MoveType.TakeEnPassant));
            Assert.That(board.IsEmptyAt("d4"), "Moved piece not removed from starting sqaure");
            Assert.That(board.IsEmptyAt("c4"), "Taken piece not removed");
            Assert.That(board["c3"].Piece.Is(Chess.Colours.Black, Chess.PieceNames.Pawn), "Moved piece not found on destination sqaure.");
        }


        [TestCase("q", Chess.PieceNames.Queen)]
        [TestCase("Q", Chess.PieceNames.Queen)]
        [TestCase("b", Chess.PieceNames.Bishop)]
        [TestCase("B", Chess.PieceNames.Bishop)]
        [TestCase("n", Chess.PieceNames.Knight)]
        [TestCase("N", Chess.PieceNames.Knight)]
        [TestCase("r", Chess.PieceNames.Rook)]
        [TestCase("R", Chess.PieceNames.Rook)]
        public void can_promote(string promotionCharacter, Chess.PieceNames name)
        {
            var asOneChar =
                "........" +
                "P......." +
                ".......k" +
                "........" +
                "........" +
                "........" +
                ".PPPPPPP" +
                "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var result = board.Move("a7-a8"+promotionCharacter);

            AssertMoveSucceeded(result, board, "a7-a8", new ChessPiece(Chess.Colours.White, name), MoveType.Promotion);

            Assert.That(result.Move.PromotedTo, Is.EqualTo(name), $"{result.Move.PromotedTo}");
        }


    }
}