using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.UnitTests.TheBoard;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Pawns
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class captures : BoardAssertions
    {
        [Test]
        public void can_enpassant()
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

            var expected = BoardLocation.List("D3", "C3");

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var result = board.Move("c2c4");
            Assert.That(result.Succeeded);

            var moves = new PawnValidMoveGenerator().For(board, "D4");

            AssertExpectedMoves(expected, moves);
        }
        [Test]
        public void cannot_take_piece_one_square_in_front_of_it()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "........" +
                "p......." +
                "PPPPPPPP" +
                "RNBQKBNR";

            var expected = BoardLocation.List();

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = new PawnValidMoveGenerator().For(board, "A2");

            AssertExpectedMoves(expected, moves);
        }
        [Test]
        public void cannot_take_piece_two_squares_in_front_of_it()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "p......." +
                "........" +
                "PPPPPPPP" +
                "RNBQKBNR";

            var expected = BoardLocation.List("A3");

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = new PawnValidMoveGenerator().For(board, "A2");

            AssertExpectedMoves(expected, moves);


        }
        [Test]
        public void can_take_pieces_diagonally_opposite()
        {
            var asOneChar =
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "........" +
                ".p.p...." +
                "PPPPPPPP" +
                "RNBQKBNR";

            var expected = BoardLocation.List("C3", "C4", "B3", "D3");

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = new PawnValidMoveGenerator().For(board, "C2");

            AssertExpectedMoves(expected, moves);
        }
    }
}