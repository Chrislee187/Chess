using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Game;

namespace chess.engine.SAN
{
    public class SanTokenParser : ISanTokenParser
    {
        public const char PromoteNotator = '=';
        public const char CheckNotator = '+';
        public const char TakeNotator = 'x';

        private const string SanCharacters = "RNBQK";
        private const string FileCharacters = "abcdefgh";
        private const string RankCharacters = "12345678";

        private static readonly Dictionary<ChessPieceName, char> SanPieceNames = new Dictionary<ChessPieceName, char>
        {
            { ChessPieceName.Pawn, ' '},
            { ChessPieceName.Rook, 'R'},
            { ChessPieceName.Knight, 'N'},
            { ChessPieceName.Bishop, 'B'},
            { ChessPieceName.Queen, 'Q'},
            { ChessPieceName.King, 'K'}
        };
        public SanTokenTypes GetTokenType(char c)
        {
            if (SanCharacters.Contains(c)) return SanTokenTypes.Piece;
            if (FileCharacters.Contains(c)) return SanTokenTypes.File;
            if (RankCharacters.Contains(c)) return SanTokenTypes.Rank;
            if (c == TakeNotator) return SanTokenTypes.Take;
            if (c == PromoteNotator) return SanTokenTypes.PromoteDelimiter;
            if (c == CheckNotator) return SanTokenTypes.Check;

            throw new ArgumentOutOfRangeException($"Unknown token: '{c}'");
        }

        public static char ToSanToken(ChessPieceName piece)
        {
            return SanPieceNames[piece];
        }
    }
    public interface ISanTokenParser
    {
        SanTokenTypes GetTokenType(char c);
    }
}