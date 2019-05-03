using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using chess.engine.Board;
using chess.engine.Movement.King;
using chess.engine.Movement.Pawn;
using chess.engine.Movement.SimpleValidators;

namespace chess.engine.Movement
{
    public interface IPathValidator
    {
        Path ValidatePath(Path possiblePath, BoardState boardState);
    }

    public delegate bool ChessBoardMovePredicate(ChessMove move, BoardState boardState);

    public interface IMoveValidator
    {
        bool ValidateMove(ChessMove move, BoardState boardState);
    }

    public class MoveValidationFactory : ReadOnlyDictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>>
    {
        
        public MoveValidationFactory() : base(new Dictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>>
        {
            // Generic move types
            { ChessMoveType.MoveOnly, new ChessBoardMovePredicate[] {new DestinationIsEmptyValidator().ValidateMove }},
            { ChessMoveType.MoveOrTake, new ChessBoardMovePredicate[] {new DestinationIsEmptyOrContainsEnemyValidator().ValidateMove}},
            { ChessMoveType.TakeOnly, new ChessBoardMovePredicate[] {new DestinationContainsEnemyMoveValidator().ValidateMove }},

            // TODO: Chess Move types shouldn't be here
            { ChessMoveType.KingMove, new ChessBoardMovePredicate[] {
                new DestinationIsEmptyValidator().ValidateMove,
                new DestinationNotUnderAttackValidator().ValidateMove}},
            { ChessMoveType.TakeEnPassant, new ChessBoardMovePredicate[] {new EnPassantTakeValidator().ValidateMove }},
            { ChessMoveType.CastleKingSide, new ChessBoardMovePredicate[] { new KingCastleValidator().ValidateMove  }},
            { ChessMoveType.CastleQueenSide, new ChessBoardMovePredicate[] { new KingCastleValidator().ValidateMove }},
            { ChessMoveType.PawnPromotion, new ChessBoardMovePredicate[] { new PawnPromotionValidator().ValidateMove }}
        })
        {}

        public IEnumerable<ChessBoardMovePredicate> Create(ChessMoveType moveType, IBoardState boardState)
        {
            if (ContainsKey(moveType))
            {
                return this[moveType];
            }

            throw new NotImplementedException($"MoveType: {moveType} not implemented");
        }
    }
}