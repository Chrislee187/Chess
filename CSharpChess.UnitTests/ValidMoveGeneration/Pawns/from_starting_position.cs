using System.Collections.Generic;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.BoardBuilderTests;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.ValidMoveGeneration.Pawns
{
    [TestFixture]
    public class from_starting_position : BoardAssertions
    {
        const int WhitePawnRank = 2;
        const int BlackPawnRank = 7;

        [Test]
        public void pawn_can_move_or_two_squares()
        {
            var board = BoardBuilder.NewGame;
            foreach (var file in Chess.Files)
            {
                var whitePawn = BoardLocation.At(file, WhitePawnRank);
                var blackPawn = BoardLocation.At(file, BlackPawnRank);
                var whiteExpected = BoardLocation.ListOf($"{whitePawn.File}{whitePawn.Rank + 1}", $"{whitePawn.File}{whitePawn.Rank + 2}");
                var blackExpected = BoardLocation.ListOf($"{blackPawn.File}{blackPawn.Rank - 1}", $"{blackPawn.File}{blackPawn.Rank - 2}");

                AssertExpectedMoves(whiteExpected, new PawnValidMoveGenerator().For(board, whitePawn));
                AssertExpectedMoves(blackExpected, new PawnValidMoveGenerator().For(board, blackPawn));
            }
        }

        [Test]
        public void pawn_cannot_take_piece_two_squares_in_front_of_it()
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

            var expected = BoardLocation.ListOf("A3");

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = new PawnValidMoveGenerator().For(board, "A2");

            AssertExpectedMoves(expected, moves);


        }
        [Test]
        public void pawn_cannot_take_piece_one_square_in_front_of_it()
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

            var expected = BoardLocation.ListOf();

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = new PawnValidMoveGenerator().For(board, "A2");

            AssertExpectedMoves(expected, moves);
        }

        [Test]
        public void pawn_can_take_pieces_diagonally_opposite()
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

            var expected = BoardLocation.ListOf("C3","C4","B3", "D3");

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = new PawnValidMoveGenerator().For(board, "C2");

            AssertExpectedMoves(expected, moves);
        }

        [Test, Ignore("TODO")]
        public void pawn_can_take_enpassant()
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

            var expected = BoardLocation.ListOf("D6", "C6");

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Colours.White);

            var moves = new PawnValidMoveGenerator().For(board, "D5");

            AssertExpectedMoves(expected, moves);
        }
    }
}