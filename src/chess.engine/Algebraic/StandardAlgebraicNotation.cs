using System;
using System.Collections.Generic;
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

        public static StandardAlgebraicNotation Parse(string notation)
        {
            return new SanBuilder().BuildFromNotation(notation);
        }

        public static bool TryParse(string notation, out StandardAlgebraicNotation an)
        {
            an = null;
            if (string.IsNullOrEmpty(notation)) return false;

            try
            {
                an = new SanBuilder().BuildFromNotation(notation);
            }
            catch
            {
                return false;
            }
            return true;
        }

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


        private class SanBuilder
        {
            private ChessPieceName? _piece;
            private ChessPieceName? _promotionPiece;
            private int? _firstFile;
            private int? _firstRank;
            private int? _secondFile;
            private int? _secondRank;
            private SanMoveTypes _moveType = SanMoveTypes.Move;

            public StandardAlgebraicNotation BuildFromNotation(string notation)
            {
                var tokens = ParseFirstToken(notation);

                ParseRemainingTokens(tokens);

                return Build();
            }

            private IEnumerable<char> ParseFirstToken(string notation)
            {
                var tokens = notation;
                var firstChar = notation[0];
                var firstCharTokenType = TokenParser.GetTokenType(firstChar);
                if (firstCharTokenType == SanTokenTypes.Piece)
                {
                    WithPiece(PieceNameMapper.FromChar(firstChar));
                    tokens = notation.Substring(1);
                }
                else if (firstCharTokenType != SanTokenTypes.File &&
                         firstCharTokenType != SanTokenTypes.Rank)
                {
                    throw new ArgumentOutOfRangeException($"Unexpected token in first character '{firstChar}'");
                }

                return tokens;
            }

            private void ParseRemainingTokens(IEnumerable<char> tokens)
            {
                foreach (var c in tokens)
                {
                    var t = TokenParser.GetTokenType(c);

                    switch (t)
                    {
                        case SanTokenTypes.Piece:
                            WithPieceToken(c);
                            break;
                        case SanTokenTypes.File:
                            WithFileToken(c);
                            break;
                        case SanTokenTypes.Rank:
                            WithRankToken(c);
                            break;
                        case SanTokenTypes.Take:
                            WithTakeMove();
                            break;
                        case SanTokenTypes.PromoteDelimiter:
                            // Ignore
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            private StandardAlgebraicNotation Build()
            {
                int? fromFile = null, fromRank = null, toFile, toRank;
                if (_secondFile.HasValue && _secondRank.HasValue)
                {
                    fromFile = _firstFile;
                    fromRank = _firstRank;
                    toFile = _secondFile;
                    toRank = _secondRank;
                }
                else
                {
                    toFile = _firstFile;
                    toRank = _firstRank;
                }
                return new StandardAlgebraicNotation(_piece ?? ChessPieceName.Pawn, fromFile, fromRank, toFile, toRank, _moveType, _promotionPiece);
            }

            private SanBuilder WithFileToken(char c)
            {
                if (!_firstFile.HasValue)
                {
                    WithFirstFile(ParseFileToken(c));
                }
                else if (!_secondFile.HasValue)
                {
                    WithSecondFile(ParseFileToken(c));
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Unexpected {nameof(SanTokenTypes.File)} token '{c}'");
                }

                return this;
            }

            private SanBuilder WithRankToken(char c)
            {
                if (_secondFile.HasValue)
                {
                    WithSecondRank(ParseRankToken(c));
                }
                else if (!_firstRank.HasValue)
                {
                    WithFirstRank(ParseRankToken(c));
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Unexpected {nameof(SanTokenTypes.Rank)} token '{c}'");
                }

                return this;
            }

            private SanBuilder WithPieceToken(char c)
            {
                if (!_promotionPiece.HasValue)
                {
                    WithPromotionPiece(c);
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Unexpected {nameof(SanTokenTypes.Piece)} token '{c}'");
                }

                return this;
            }

            private SanBuilder WithTakeMove()
            {
                _moveType = SanMoveTypes.Take;
                return this;
            }

            private SanBuilder WithPiece(ChessPieceName c)
            {
                _piece = c;
                return this;
            }

            private SanBuilder WithPromotionPiece(char c)
            {
                _promotionPiece = PieceNameMapper.FromChar(c);
                return this;
            }

            private SanBuilder WithFirstFile(int tokenValue)
            {
                _firstFile = tokenValue;
                return this;
            }

            private SanBuilder WithFirstRank(int tokenValue)
            {
                _firstRank = tokenValue;
                return this;
            }

            private SanBuilder WithSecondFile(int tokenValue)
            {
                _secondFile = tokenValue;
                return this;
            }

            private SanBuilder WithSecondRank(int tokenValue)
            {
                _secondRank = tokenValue;
                return this;
            }

            private static int ParseRankToken(char c) => int.Parse(c.ToString());

            private static int ParseFileToken(char c) => c - 96; // TODO: assuming ascii 'a' is 97

        }
    }

}