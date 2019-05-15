using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Chess.Pieces;
using chess.engine.Extensions;
using chess.engine.Game;

namespace chess.engine.Algebraic
{
    [DebuggerDisplay("{DebuggerDisplayText}")]
    public class StandardAlgebraicNotation
    {
        public enum CastleSide { None, King, Queen }

        public CastleSide CastleMove = CastleSide.None;
        public static StandardAlgebraicNotation KingSideCastle => new StandardAlgebraicNotation(CastleSide.King);
        public static StandardAlgebraicNotation QueenSideCastle => new StandardAlgebraicNotation(CastleSide.Queen);
        private string DebuggerDisplayText => ToNotation();
        private string _originalNotation;
        public SanMoveTypes MoveType { get; }
        public ChessPieceName? PromotionPiece { get; }
        public ChessPieceName Piece { get; }

        // NOTE: Added X/Y because I still keep getting them the wrong way round, I blame the
        // phrase "rank and file" for it! :)
        public int ToFileX { get; }
        public int ToRankY { get; }

        public int? FromFileX { get; }
        public int? FromRankY { get; }

        public bool HaveFrom => FromFileX.HasValue && FromRankY.HasValue;

        public bool InCheck { get; }


        public static StandardAlgebraicNotation Parse(string notation)
        {
            var an = new SanBuilder().BuildFrom(notation);

            if (an.CastleMove == CastleSide.None && an.ToNotation() != notation)
            {
                throw new Exception($"Notation parse mismatch: {notation} != {an.ToNotation()}");
            }
            return an;
        }

        public static bool TryParse(string notation, out StandardAlgebraicNotation an)
        {
            an = null;
            if (string.IsNullOrEmpty(notation)) return false;

            try
            {
                an = Parse(notation);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static StandardAlgebraicNotation ParseFromGameMove(IBoardState<ChessPieceEntity> boardState, BoardMove move)
        {
            return new SanBuilder().BuildFrom(boardState, move);
        }
        public StandardAlgebraicNotation(ChessPieceName piece,
            int? fromFileX, int? fromRankY, int toFileX, int toRankY,
            string originalNotation,
            SanMoveTypes moveType = SanMoveTypes.Move,
            ChessPieceName? promotionPiece = null, bool inCheck = false)
        {
            _originalNotation = originalNotation;
            Piece = piece;
            FromFileX = fromFileX;
            FromRankY = fromRankY;
            ToFileX = toFileX;
            ToRankY = toRankY;
            MoveType = moveType;
            PromotionPiece = promotionPiece;
            InCheck = inCheck;
        }

        private StandardAlgebraicNotation(CastleSide side)
        {
            Piece = ChessPieceName.King;
            CastleMove = side;
            FromFileX = King.StartPositionFor(Colours.White).X;

            ToFileX = side == CastleSide.Queen
                ? 3
                : 7;
        }

        public string ToNotation()
        {
            if (CastleMove != CastleSide.None)
            {
                return CastleMove == CastleSide.King
                    ? "O-O"
                    : "O-O-O";
            }
            var piece = SanTokenParser.ToSanToken(Piece);

            var from = buildChessLocation(FromFileX, FromRankY);
            var move = MoveType == SanMoveTypes.Take ? SanTokenParser.TakeNotator.ToString() : "";
            var to = buildChessLocation(ToFileX, ToRankY);
            var extra = buildExtra(PromotionPiece);
            var inCheck = InCheck ? SanTokenParser.CheckNotator.ToString() : "";
            return $"{piece}{from}{move}{to}{extra}{inCheck}".Trim();
        }

        private string buildExtra(ChessPieceName? promotionPiece)
        {
            if (promotionPiece.HasValue)
            {
                return $"{SanTokenParser.PromoteNotator.ToString()}{PieceNameMapper.ToChar(promotionPiece.Value, Colours.White)}";
            }

            return string.Empty;
        }

        private string buildChessLocation(int? fromFile, int? fromRank)
        {
            var file = "";
            var rank = "";

            if (fromFile.HasValue)
            {
                var tmp = BoardLocation.At(fromFile.Value, 1);
                file = tmp.ToChessCoord()[0].ToString().ToLower();
            }
            if (fromRank.HasValue)
            {
                var tmp = BoardLocation.At(1, fromRank.Value);
                rank = tmp.ToChessCoord()[1].ToString().ToLower();
            }

            return $"{file}{rank}";
        }


        #region Token Parsing
        private class SanTokenParser
        {
            public const char PromoteNotator = '=';
            public const char CheckNotator = '+';
            public const char TakeNotator = 'x';

            private const string SanCharacters = "RNBQK";
            private const string FileCharacters = "abcdefgh";
            private const string RankCharacters = "12345678";

            private static readonly Dictionary<ChessPieceName, char> _sanPieceNames = new Dictionary<ChessPieceName, char>
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
                return _sanPieceNames[piece];
            }
        }

        private enum SanTokenTypes
        {
            Piece, Rank, File, Take, PromoteDelimiter,
            Check
        }
        #endregion


        private class SanBuilder
        {
            private readonly SanTokenParser _tokenParser = new SanTokenParser();

            private ChessPieceName? _piece;
            private ChessPieceName? _promotionPiece;
            private int? _firstFile;
            private int? _firstRank;
            private int? _secondFile;
            private int? _secondRank;
            private SanMoveTypes _moveType = SanMoveTypes.Move;

            private string _originalNotation;
            private bool _inCheck;
            public StandardAlgebraicNotation BuildFrom(IBoardState<ChessPieceEntity> boardState, BoardMove move)
            {
                var fromItem = boardState.GetItem(move.From);

                var piece = fromItem.Item.Piece;
                int? fromFile = null;
                int? fromRank = null;
                int toFile = move.To.X;
                int toRank = move.To.Y;
                var moveType = boardState.IsEmpty(move.To) ? SanMoveTypes.Move : SanMoveTypes.Take;
                var extra = "";

                // Are they any other pieces, 
                //      of same type as the from item
                //      how can also move to the same location
                var otherPieces = boardState.GetAllItems()
                    .Where(i => !i.Location.Equals(move.From))
                    .Where(i => i.Item.Is(fromItem.Item.Player, fromItem.Item.Piece))
                    .Where(i => i.Paths.ContainsMoveTo(move.To));

                if (otherPieces.Any())
                {
                    fromFile = move.From.X;
                    otherPieces = otherPieces
                        .Where(i => i.Location.X == fromFile);
                }
                if (otherPieces.Any())
                {
                    fromRank = move.From.Y;
                    otherPieces = new List<LocatedItem<ChessPieceEntity>>();
                }

                if (otherPieces.Any())
                {
                    throw new NotImplementedException($"Unable to disambiguate {move}");
                }

                if (piece == ChessPieceName.Pawn && moveType == SanMoveTypes.Take)
                {
                    fromFile = fromItem.Location.X;
                }

                ChessPieceName? promotionPiece = null;
                if (move.ExtraData is ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData)
                {
                    var data = move.ExtraData as ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData;
                    promotionPiece = data.PieceName;
                }

                // TODO: Whether the move puts the enemy in check
                
                // Pullout DoeMoveLeaveUsInCheck() from ChessRefershAllPaths as seperate service and reuse here
                var inCheck = false;



                return new StandardAlgebraicNotation(piece, fromFile, fromRank, toFile, toRank, move.ToChessCoords(), moveType, promotionPiece, inCheck );
            }

            public StandardAlgebraicNotation BuildFrom(string notation)
            {
                if (notation.StartsWith("O") || notation.StartsWith("0"))
                {
                    // TODO: Cheating here should parse it properly
                    if (notation.Length == 3)
                    {
                        return StandardAlgebraicNotation.KingSideCastle;
                    }

                    if (notation.Length == 5)
                    {
                        return StandardAlgebraicNotation.QueenSideCastle;
                    }

                    throw new ArgumentException($"{notation} is not a valid castle notation");
                }
                _originalNotation = notation;
                var tokens = ParseFirstToken(notation);

                ParseRemainingTokens(tokens);

                return Build();
            }

            private IEnumerable<char> ParseFirstToken(string notation)
            {
                var tokens = notation;
                var firstChar = notation[0];
                var firstCharTokenType = _tokenParser.GetTokenType(firstChar);

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
                    var t = _tokenParser.GetTokenType(c);

                    if (_tokenActionFactory.TryGetValue(t, out var action))
                    {
                        action(this, c);
                    }
                    else
                    { 
                        throw new ArgumentOutOfRangeException($"No action found for token type: {t}");
                    }
                }
            }

            private StandardAlgebraicNotation Build()
            {
                int? fromFile = null, fromRank = null;
                int toFile, toRank;
                if (_secondFile.HasValue && _secondRank.HasValue)
                {
                    fromFile = _firstFile;
                    fromRank = _firstRank;
                    toFile = _secondFile.Value;
                    toRank = _secondRank.Value;
                }
                else
                {
                    toFile = _firstFile.Value;
                    toRank = _firstRank.Value;
                }
                return new StandardAlgebraicNotation(_piece ?? ChessPieceName.Pawn, fromFile, fromRank, toFile, toRank, _originalNotation, _moveType, _promotionPiece, _inCheck);
            }

            private readonly IDictionary<SanTokenTypes, Action<SanBuilder, char>> _tokenActionFactory = new Dictionary<SanTokenTypes, Action<SanBuilder, char>>
            {
                {SanTokenTypes.Piece, (b,c) => b.WithPieceToken(c) },
                {SanTokenTypes.File, (b,c) => b.WithFileToken(c) },
                {SanTokenTypes.Rank, (b,c) => b.WithRankToken(c) },
                {SanTokenTypes.Take, (b,c) => b.WithTakeMove(c) },
                {SanTokenTypes.PromoteDelimiter, (b, c) => { } },
                {SanTokenTypes.Check, (b, c) => b.WithCheck(c)  }
            };

            private void WithCheck(char c)
            {
                _inCheck = true;
            }

            private void WithFileToken(char c)
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
            }

            private void WithRankToken(char c)
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
            }

            private void WithPieceToken(char c)
            {
                if (!_promotionPiece.HasValue)
                {
                    WithPromotionPiece(c);
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Unexpected {nameof(SanTokenTypes.Piece)} token '{c}'");
                }
            }

            private void WithTakeMove(char c) => _moveType = SanMoveTypes.Take;

            private void WithPiece(ChessPieceName c) => _piece = c;

            private void WithPromotionPiece(char c) => _promotionPiece = PieceNameMapper.FromChar(c);

            private void WithFirstFile(int tokenValue) => _firstFile = tokenValue;

            private void WithFirstRank(int tokenValue) => _firstRank = tokenValue;

            private void WithSecondFile(int tokenValue) => _secondFile = tokenValue;

            private void WithSecondRank(int tokenValue) => _secondRank = tokenValue;

            private static int ParseRankToken(char c) => int.Parse(c.ToString());

            private static int ParseFileToken(char c) => c - 96; // TODO: assuming ascii 'a' is 97
        }
    }

}