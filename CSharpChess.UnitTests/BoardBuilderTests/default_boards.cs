using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;
using NUnit.Framework;

namespace CSharpChess.UnitTests.BoardBuilderTests
{
    [TestFixture]
    public class default_boards
    {
        [Test]
        public void empty_board()
        {
            var board = BoardBuilder.EmptyBoard;

            Assert.True(board.Pieces.All(p => p.Piece.Equals(Chess.Pieces.Blank)));
        }

        [Test]
        public void newgame_board()
        {
            var board = BoardBuilder.NewGame;

            AssertNewGameBoard(board);
        }

        private static void AssertNewGameBoard(ChessBoard board)
        {
            var view = new OneCharBoard(board);
            var ranks = view.Ranks.ToList();
            ranks.ForEach(Console.WriteLine);
            Assert.That(ranks[7], Is.EqualTo("rnbqkbnr"));
            Assert.That(ranks[6], Is.EqualTo("pppppppp"));
            Assert.That(ranks[5], Is.EqualTo("........"));
            Assert.That(ranks[4], Is.EqualTo("........"));
            Assert.That(ranks[3], Is.EqualTo("........"));
            Assert.That(ranks[2], Is.EqualTo("........"));
            Assert.That(ranks[1], Is.EqualTo("PPPPPPPP"));
            Assert.That(ranks[0], Is.EqualTo("RNBQKBNR"));
        }

        [Test]
        public void custom_board_can_be_built_using_onechar_notation()
        {
            var asOneChar = 
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "........" +
                "........" +
                "PPPPPPPP" +
                "RNBQKBNR";

            var customboard = ChessBoardHelper.OneCharBoardToBoardPieces(asOneChar);
            var board = new ChessBoard(customboard);

            AssertNewGameBoard(board);
        }
    }

    public class ChessBoardHelper
    {
        private static string _validOneChars = ". prnbqkPRNBQK";

        public static IEnumerable<BoardPiece> OneCharBoardToBoardPieces(string asOneChar)
        {
            var invalidChars = asOneChar.Where(c => !_validOneChars.Contains(c)).ToArray();

            AssertValidRepresentation(asOneChar, invalidChars);

            var pieces = new List<BoardPiece>();
            foreach (var rank in Chess.Ranks)
            {
                foreach (var file in Chess.Files)
                {
                    var oneCharPiece = asOneChar[RankAndFileToOneCharIndex(rank, file)];
                    var colour = OneCharBoard.PieceColour(oneCharPiece);
                    var name = OneCharBoard.PieceName(oneCharPiece);

                    pieces.Add(new BoardPiece(file, rank, new ChessPiece(colour, name)));
                }
                
            }
            return pieces;
        }

        private static void AssertValidRepresentation(string asOneChar, char[] invalidChars)
        {
            if (invalidChars.Any())
                throw new ArgumentException(
                    $"Invalid characters found in OneChar representation; '{new string(invalidChars.ToArray())}'",
                    nameof(asOneChar));

            if (asOneChar.ToCharArray().Length != 64)
                throw new ArgumentException("OneChar board representation must contain exactly 64 char's", nameof(asOneChar));
        }

        private static int RankAndFileToOneCharIndex(int rank, int file)
        {
            var idx = (8 - rank)*8 + (file - 1);
            return idx;
        }
    }
}