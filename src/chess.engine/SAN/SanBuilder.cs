﻿using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Pieces;

namespace chess.engine.SAN
{
    public class SanBuilder : ISanBuilder
    {
        private readonly ISanTokenParser _sanTokenParser;
        private readonly ICheckDetectionService _checkDetectionService;

        public SanBuilder(
            ICheckDetectionService checkDetectionService,
            ISanTokenParser sanTokenParser
        )
        {
            _checkDetectionService = checkDetectionService;
            _sanTokenParser = sanTokenParser;
        }

        private ChessPieceName? _piece;
        private ChessPieceName? _promotionPiece;
        private int? _firstFile;
        private int? _firstRank;
        private int? _secondFile;
        private int? _secondRank;
        private SanMoveTypes _moveType = SanMoveTypes.Move;

        private string _originalNotation;
        private bool _inCheck;
        public StandardAlgebraicNotation BuildFrom(IBoardState<ChessPieceEntity> boardState, BoardMove move, bool performCheckTest = false)
        {
            var fromItem = boardState.GetItem(move.From);

            var piece = fromItem.Item.Piece;
            int? fromFile = null;
            int? fromRank = null;
            var toFile = move.To.X;
            var toRank = move.To.Y;
            var moveType = boardState.IsEmpty(move.To) ? SanMoveTypes.Move : SanMoveTypes.Take;

            // Are they any other pieces, 
            //      of same type as the from item
            //      how can also move to the same location
            var otherPieces = boardState.GetItems()
                .Where(i => !i.Location.Equals(move.From))
                .Where(i => i.Item.Is(fromItem.Item.Player, fromItem.Item.Piece))
                .ThatCanMoveTo(move.To);

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
            if (move.ExtraData is ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData data)
            {
                promotionPiece = data.PieceName;
            }

            // Note: This may has a small performance impact when parsing lots of games, approx 20ms per game on my rig,
            // it just produces the check notator '+' at the
            // end of a move that causes check, it's for display information only and not used at all by any processing.
            // Make it optional so that things display moves can show it but things just parsing moves doesn't need it
            // 
            var inCheck = performCheckTest && IsEnemyInCheck(boardState, fromItem);
            return new StandardAlgebraicNotation(piece, fromFile, fromRank, toFile, toRank, moveType, promotionPiece, inCheck );
        }

        private bool IsEnemyInCheck(IBoardState<ChessPieceEntity> boardState, LocatedItem<ChessPieceEntity> fromItem)
        {
            var enemyKingLoc = boardState.GetItems()
                .FindItem((int) fromItem.Item.Player.Enemy(), (int) ChessPieceName.King);

            return _checkDetectionService
                .IsLocationUnderAttack(boardState,
                    enemyKingLoc.Location, enemyKingLoc.Item.Player);
        }

        // TODO: Was doesn't this use the TryParse pattern
        public StandardAlgebraicNotation BuildFrom(string notation)
        {
            if (notation.StartsWith("O-O") || notation.StartsWith("0-0"))
            {
                // TODO: Cheating here should parse it properly
                if (notation.Length >= 5)
                {
                    return StandardAlgebraicNotation.QueenSideCastle;
                }

                if (notation.Length >= 3)
                {
                    return StandardAlgebraicNotation.KingSideCastle;
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
            var firstCharTokenType = _sanTokenParser.GetTokenType(firstChar);

            if (firstCharTokenType == SanTokenTypes.Piece)
            {
                WithPiece(ChessPieceNameMapper.FromChar(firstChar));
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
                var t = _sanTokenParser.GetTokenType(c);

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
            if (!_secondFile.HasValue && !_secondRank.HasValue)
            {
                if(!_firstRank.HasValue || !_firstFile.HasValue) throw new ArgumentOutOfRangeException($"Rank or File is missing");

                toRank = _firstRank.Value;
                toFile = _firstFile.Value;

            } else 
            {

                fromFile = _firstFile;
                fromRank= _firstRank;
                // ReSharper disable once PossibleInvalidOperationException
                toFile = _secondFile.Value;
                // ReSharper disable once PossibleInvalidOperationException
                toRank = _secondRank.Value;
            }
            return new StandardAlgebraicNotation(_piece ?? ChessPieceName.Pawn, fromFile, fromRank, toFile, toRank, _moveType, _promotionPiece, _inCheck);
        }

        private readonly IDictionary<SanTokenTypes, Action<SanBuilder, char>> _tokenActionFactory = new Dictionary<SanTokenTypes, Action<SanBuilder, char>>
        {
            {SanTokenTypes.Piece, (b,c) => b.WithPieceToken(c) },
            {SanTokenTypes.File, (b,c) => b.WithFileToken(c) },
            {SanTokenTypes.Rank, (b,c) => b.WithRankToken(c) },
            {SanTokenTypes.Take, (b,c) => b.WithTakeMove() },
            {SanTokenTypes.PromoteDelimiter, (b, c) => { } },
            {SanTokenTypes.Check, (b, c) => b.WithCheck()  }
        };
            
        private void WithCheck()
        {
            _inCheck = true;
        }

        private void WithFileToken(char c)
        {
            if (!_firstFile.HasValue && !_firstRank.HasValue)
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
            if (!_firstRank.HasValue && !_secondFile.HasValue)
            {
                WithFirstRank(ParseRankToken(c));
            }
            else if (!_secondRank.HasValue)
            {
                WithSecondRank(ParseRankToken(c));
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

        private void WithTakeMove() => _moveType = SanMoveTypes.Take;
        private void WithPiece(ChessPieceName c) => _piece = c;
        private void WithPromotionPiece(char c) => _promotionPiece = ChessPieceNameMapper.FromChar(c);
        private void WithFirstFile(int tokenValue) => _firstFile = tokenValue;
        private void WithFirstRank(int tokenValue) => _firstRank = tokenValue;
        private void WithSecondFile(int tokenValue) => _secondFile = tokenValue;
        private void WithSecondRank(int tokenValue) => _secondRank = tokenValue;
        private static int ParseRankToken(char c) => Int32.Parse(c.ToString());
        private static int ParseFileToken(char c) => c - 96; // TODO: assuming ascii 'a' is (and always will be) 97, also assumes UK SAN for pieces identifiers
    }
    public interface ISanBuilder
    {
        StandardAlgebraicNotation BuildFrom(IBoardState<ChessPieceEntity> boardState, BoardMove move);
        StandardAlgebraicNotation BuildFrom(string notation);
    }

}