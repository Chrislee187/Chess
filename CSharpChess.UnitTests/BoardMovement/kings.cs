using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;
using CSharpChess.UnitTests.Helpers;
using CSharpChess.ValidMoves;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardMovement
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class kings : BoardAssertions
    {
        [Test]
        public void can_move_with_a_king()
        {
            const string asOneChar = ".......k" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "K.......";
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);

            var result = board.Move("a1a2");

            AssertMoveSucceeded(result, board, "a1a2", new ChessPiece(Chess.Board.Colours.White, Chess.Board.PieceNames.King));
        }

        [Test]
        public void can_take_with_a_king()
        {
            const string asOneChar = ".......k" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "p......." +
                                     "K.......";
            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);

            var result = board.Move("a1a2");

            AssertTakeSucceeded(result, board, "a1a2", new ChessPiece(Chess.Board.Colours.White, Chess.Board.PieceNames.King));
        }


        [TestCase("E1C1", "A1D1")]
        [TestCase("E1G1", "H1F1")]
        [TestCase("E8C8", "A8D8")]
        [TestCase("E8G8", "H8F8")]
        public void can_castle(string kingMove, string expectedRookMove)
        {
            const string asOneChar = "r...k..r" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "R...K..R";

            var km = (ChessMove)kingMove;
            var colour = km.From.Rank == 1 ? Chess.Board.Colours.White : Chess.Board.Colours.Black;
            var board = BoardBuilder.CustomBoard(asOneChar, colour);

            var rm = (ChessMove)expectedRookMove;
            var moveResult = board.Move(km);

            Assert.That(moveResult.Succeeded, $"Failed: {kingMove}.");
            Assert.That(board[km.To].Piece.Is(colour, Chess.Board.PieceNames.King));
            Assert.That(board[rm.To].Piece.Is(colour, Chess.Board.PieceNames.Rook));
            Assert.That(board.IsEmptyAt(km.From));
            Assert.That(board.IsEmptyAt(rm.From));

        }

        [Test]
        public void cannot_castle_through_check()
        {
            const string asOneChar = "r...k.r." +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "R...K..R";

            var km = (ChessMove)"E1G1";
            var colour = km.From.Rank == 1 ? Chess.Board.Colours.White : Chess.Board.Colours.Black;
            var board = BoardBuilder.CustomBoard(asOneChar, colour);

            var moves = board.MovesFor(km.From).ToList();
            Assert.That(moves.Any());
            var moveResult = board.Move(km);
            Console.WriteLine(new MediumConsoleBoard(board).Build().ToString());
            Assert.False(moveResult.Succeeded, $"Failed: {km} move through check");
        }

        [Test]
        public void cannot_move_into_check()
        {
            const string asOneChar = "...rkr.." +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "...n...." +
                                     ".R..K.R.";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.White);

            var at = BoardLocation.At("E1");
            var expected = new[] {"D1", "E2"};
            var notExpected = new[] {"F1", "F2"};
            var chessMoves = board.MovesFor(at).Moves().ToList();

            AssertMovesContains(chessMoves, expected, MoveType.Move);
            AssertMovesDoesNotContain(chessMoves, notExpected, MoveType.Move);
            // TODO: Assert does not contain the others
            Assert.That(chessMoves.Count(), Is.EqualTo(2), chessMoves.ToCSV());
        }

        [Test]
        public void move_must_resolve_check()
        {
            const string asOneChar = ".R..k..r" +
                                     "........" +
                                     "...B...." +
                                     "...B...." +
                                     "........" +
                                     "........" +
                                     "....R..." +
                                     "....K...";

            var board = BoardBuilder.CustomBoard(asOneChar, Chess.Board.Colours.Black);
            Assert.That(board.GameState, Is.EqualTo(Chess.GameState.BlackKingInCheck));
            var result = board.Move("H8H7");
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Message, Is.Not.Empty);
        }
    }

}