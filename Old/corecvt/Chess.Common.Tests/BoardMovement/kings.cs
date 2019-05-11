using System;
using Chess.Common.Tests.Helpers;
using CSharpChess;
using CSharpChess.Extensions;
using CSharpChess.Movement;
using CSharpChess.System;
using NUnit.Framework;

namespace Chess.Common.Tests.BoardMovement
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
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("a1a2");

            AssertMoveSucceeded(result, board, "a1a2", new ChessPiece(Colours.White, PieceNames.King));
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
            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var result = board.Move("a1a2");

            AssertTakeSucceeded(result, board, "a1a2", new ChessPiece(Colours.White, PieceNames.King));
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

            var km = (Move)kingMove;
            var colour = km.From.Rank == 1 ? Colours.White : Colours.Black;
            var board = BoardBuilder.CustomBoard(asOneChar, colour);

            var rm = (Move)expectedRookMove;
            var moveResult = board.Move(km);

            Assert.That(moveResult.Succeeded, $"Failed: {kingMove}.");
            Assert.That(board[km.To].Piece.Is(colour, PieceNames.King));
            Assert.That(board[rm.To].Piece.Is(colour, PieceNames.Rook));
            Assert.That(board.IsEmptyAt(km.From));
            Assert.That(board.IsEmptyAt(rm.From));
            Console.WriteLine(kingMove);
            Console.WriteLine(board.ToAsciiBoard());
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

            var km = (Move)"E1G1";
            var colour = km.From.Rank == 1 ? Colours.White : Colours.Black;
            var board = BoardBuilder.CustomBoard(asOneChar, colour);

            var moves = board.RemoveMovesThatLeaveBoardInCheck(km.From).ToList();
            Assert.That(moves.Any());
            var moveResult = board.Move(km);
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

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            var at = BoardLocation.At("E1");
            var expected = new[] {"D1", "E2"};
            var notExpected = new[] {"F1", "F2"};
            var chessMoves = board.RemoveMovesThatLeaveBoardInCheck(at).Moves().ToList();

            AssertMovesContains(chessMoves, expected, MoveType.Move);
            AssertMovesDoesNotContain(chessMoves, notExpected, MoveType.Move);
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

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.Black);
            Assert.That(board.GameState, Is.EqualTo(GameState.BlackKingInCheck));
            var result = board.Move("H8H7");
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        public void detects_checkmate()
        {
            const string asOneChar = "....k..." +
                                     ".R.....R" +
                                     "...B...." +
                                     "...B...." +
                                     "........" +
                                     "........" +
                                     "........" +
                                     "....K...";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            var result = board.Move("B7B8");
            Assert.That(result.Succeeded, Is.True);
            Assert.That(board.GameState, Is.EqualTo(GameState.CheckMateWhiteWins));
        }

        [Test]
        public void Bugfix1()
        {
            const string asOneChar = "r...k..r" +
                                     ".ppb.ppp" +
                                     "p..b...." +
                                     "........" +
                                     "...N.P.." +
                                     "..P.B..." +
                                     "PP....PP" +
                                     "R...KB.R";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);
            var result = board.Move("E1-C1");
            Assert.That(result.Succeeded, Is.True, board.ToAsciiBoard());
            Assert.That(board["C1"].Piece.Is(Colours.White, PieceNames.King), board.ToAsciiBoard());
            Assert.That(board["D1"].Piece.Is(Colours.White, PieceNames.Rook), board.ToAsciiBoard());
            Assert.That(board.IsEmptyAt("A1"), board.ToAsciiBoard());
            Console.WriteLine(board.ToAsciiBoard());
        }


    }

}