using System.Linq;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class pawns : BoardAssertions
    {
        [Test]
        public void can_move_two_forward()
        {
            var board = BoardBuilder.NewGame;

            var result = board.Move("D2-D4");

            Assert.True(result.Succeeded);
            Assert.True(board.IsEmptyAt("d2"));
            Assert.True(board.IsNotEmptyAt("d4"));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.MoveType, Is.EqualTo(MoveType.Move));

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
            Assert.That(board.IsEmptyAt("c4"), "Taken piece not removed");
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
                "........" +
                "........" +
                "........" +
                "........" +
                ".PPPPPPP" +
                "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moveResult = board.Move("a7-a8"+promotionCharacter);

            Assert.That(moveResult.Succeeded);
            Assert.That(moveResult.MoveType, Is.EqualTo(MoveType.Promotion));
            Assert.That(moveResult.Move.PromotedTo, Is.EqualTo(name));

            Assert.That(board["a8"].Piece.Is(Chess.Colours.White));
            Assert.That(board["a8"].Piece.Is(name));
        }


    }
}