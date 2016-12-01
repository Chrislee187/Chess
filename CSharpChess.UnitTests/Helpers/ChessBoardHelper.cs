using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.UnitTests.Helpers
{
    public static class ChessBoardHelper
    {
        private static string _validOneChars = ". prnbqkPRNBQK";

        public static IEnumerable<BoardPiece> OneCharBoardToBoardPieces(string asOneChar)
        {
            var validChars = AssertValidRepresentation(asOneChar);

            var pieces = new List<BoardPiece>();
            foreach (var rank in Chess.Ranks)
            {
                foreach (var file in Chess.Files)
                {
                    var oneCharPiece = validChars[RankAndFileToOneCharIndex(rank, file)];
                    var colour = OneCharBoard.PieceColour(oneCharPiece);
                    var name = OneCharBoard.PieceName(oneCharPiece);

                    pieces.Add(new BoardPiece(file, rank, new ChessPiece(colour, name)));
                }
                
            }
            return pieces;
        }

        private static string AssertValidRepresentation(string asOneChar)
        {
            var ignoredChars = new char[]  { '\n', '\t', '\r', ' '};
            var trimmed = asOneChar.Trim(ignoredChars);
            var invalidChars = trimmed.Where(c => !_validOneChars.Contains(c)).ToArray();

            if (invalidChars.Any())
                throw new ArgumentException(
                    $"Invalid characters found in OneChar representation; '{new string(invalidChars.ToArray())}'",
                    nameof(asOneChar));

            if (trimmed.ToCharArray().Length != 64)
                throw new ArgumentException("OneChar board representation must contain exactly 64 valid char's excluding whitespace", nameof(asOneChar));

            return trimmed;
        }

        private static int RankAndFileToOneCharIndex(int rank, int file)
        {
            var idx = (8 - rank)*8 + (file - 1);
            return idx;
        }
    }
}