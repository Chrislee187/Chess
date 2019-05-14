using System;
using System.Linq;
using chess.engine.Chess;
using chess.engine.Chess.Pieces;

namespace chess.engine.Algebraic
{

    public class StandardAlgebraicNotation
    {
        private static readonly SanTokenParser TokenParser = new SanTokenParser();
        public SanMoveTypes MoveType { get; }
        public ChessPieceName? PromotionPiece { get; }
        public ChessPieceName Piece { get; }
        public int? ToFile { get; }
        public int? ToRank { get; }

        public int? FromFile { get; }
        public int? FromRank { get; }

        public static bool TryParse(string notation, out StandardAlgebraicNotation an)
        {
            an = null;
            if (string.IsNullOrEmpty(notation)) return false;

            ChessPieceName? piece = null;
            ChessPieceName? promotionPiece = null;
            int? firstFile = null;
            int? firstRank = null;
            int? secondFile = null;
            int? secondRank = null;
            var moveType = SanMoveTypes.Move;

            var tokens = notation;
            var firstChar = notation[0];
            var firstCharTokenType = TokenParser.GetTokenType(firstChar);
            if (firstCharTokenType == SanTokenTypes.Piece)
            {
                piece = PieceNameMapper.FromChar(firstChar);
                tokens = notation.Substring(1);
            } else if (firstCharTokenType != SanTokenTypes.File &&
                       firstCharTokenType != SanTokenTypes.Rank)
            {
                throw new ArgumentOutOfRangeException($"Unexpected token in first character '{firstChar}'");
            }

            foreach (var c in tokens)
            {
                var t = TokenParser.GetTokenType(c);

                // TODO: Refactor this into something a little nicer, methods using a builder perhaps?
                switch (t)
                {
                    case SanTokenTypes.Piece:
                        if (!promotionPiece.HasValue)
                        {
                            promotionPiece = PieceNameMapper.FromChar(c);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException($"Unexpected {nameof(SanTokenTypes.Piece)} token '{c}'");
                        }
                        break;
                    case SanTokenTypes.File:
                        if (!firstFile.HasValue)
                        {
                            firstFile = ParseFileToken(c); 
                        }
                        else if (!secondFile.HasValue)
                        {
                            secondFile = ParseFileToken(c);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException($"Unexpected {nameof(SanTokenTypes.File)} token '{c}'");
                        }
                        break;
                    case SanTokenTypes.Rank:
                        if (secondFile.HasValue)
                        {
                            secondRank = ParseRankToken(c);
                        }
                        else if (!firstRank.HasValue)
                        {
                            firstRank = ParseRankToken(c);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException($"Unexpected {nameof(SanTokenTypes.Rank)} token '{c}'");
                        }
                        break;
                    case SanTokenTypes.Take:
                        moveType = SanMoveTypes.Take;
                        break;
                    case SanTokenTypes.PromoteDelimiter:
                        // Ignore
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            int? fromFile = null, fromRank = null, toFile, toRank;
            if (secondFile.HasValue && secondRank.HasValue)
            {
                fromFile = firstFile;
                fromRank = firstRank;
                toFile = secondFile;
                toRank = secondRank;
            }
            else
            {
                toFile = firstFile;
                toRank = firstRank;
            }

            an = new StandardAlgebraicNotation(piece ?? ChessPieceName.Pawn, fromFile, fromRank, toFile, toRank, moveType, promotionPiece);
            return true;
        }

        private static int ParseRankToken(char c) => int.Parse(c.ToString());

        private static int ParseFileToken(char c) => c - 96; // TODO: assuming ascii 'a' is 97

        public StandardAlgebraicNotation(
            ChessPieceName piece,
            int? fromFile, int? fromRank, int? toFile, int? toRank, 
            SanMoveTypes moveType = SanMoveTypes.Move,
            ChessPieceName? promotionPiece = null)
        {
            Piece = piece;
            FromFile = fromFile;
            FromRank = fromRank;
            ToFile = toFile;
            ToRank = toRank;
            MoveType = moveType;
            PromotionPiece = promotionPiece;
        }

        #region Token Parsing
        private class SanTokenParser
        {
            private const string SanCharacters = "RNBQK";
            private const string FileCharacters = "abcdefgh";
            private const string RankCharacters = "12345678";

            public SanTokenTypes GetTokenType(char c)
            {
                if (SanCharacters.Contains(c)) return SanTokenTypes.Piece;
                if (FileCharacters.Contains(c)) return SanTokenTypes.File;
                if (RankCharacters.Contains(c)) return SanTokenTypes.Rank;
                if (c == 'x') return SanTokenTypes.Take;
                if (c == '+') return SanTokenTypes.PromoteDelimiter;

                throw new ArgumentOutOfRangeException($"Unknown token: '{c}'");
            }

        }

        private enum SanTokenTypes
        {
            Piece, Rank, File, Take, PromoteDelimiter
        }
        #endregion
    }

}