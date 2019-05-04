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
        Path ValidatePath(Path possiblePath, IBoardState boardState);
    }

    public delegate bool ChessBoardMovePredicate(ChessMove move, IBoardState boardState);

    public interface IMoveValidator
    {
        bool ValidateMove(ChessMove move, IBoardState boardState);
    }

    public class MoveValidationFactory : ReadOnlyDictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>>
    {
        
        public MoveValidationFactory() : base(new Dictionary<ChessMoveType, IEnumerable<ChessBoardMovePredicate>>
        {
            // Generic move types
            { ChessMoveType.MoveOnly, new ChessBoardMovePredicate[] {(move, boardState) => new DestinationIsEmptyValidator().ValidateMove(move, boardState) }},
            { ChessMoveType.MoveOrTake, new ChessBoardMovePredicate[] {(move, boardState) => new DestinationIsEmptyOrContainsEnemyValidator().ValidateMove(move, boardState)}},
            { ChessMoveType.TakeOnly, new ChessBoardMovePredicate[] {(move, boardState) => new DestinationContainsEnemyMoveValidator().ValidateMove(move, boardState) }},

            // TODO: Chess Move types shouldn't be here
            { ChessMoveType.KingMove, new ChessBoardMovePredicate[] {
                (move, boardState) => new DestinationIsEmptyValidator().ValidateMove(move, boardState),
                (move, boardState) => new DestinationNotUnderAttackValidator().ValidateMove(move, boardState)}},
            { ChessMoveType.TakeEnPassant, new ChessBoardMovePredicate[] {(move, boardState) => new EnPassantTakeValidator().ValidateMove(move, boardState) }},
            { ChessMoveType.CastleKingSide, new ChessBoardMovePredicate[] { (move, boardState) => new KingCastleValidator().ValidateMove(move, boardState)  }},
            { ChessMoveType.CastleQueenSide, new ChessBoardMovePredicate[] { (move, boardState) => new KingCastleValidator().ValidateMove(move, boardState) }},
            { ChessMoveType.PawnPromotion, new ChessBoardMovePredicate[] { (move, boardState) => new PawnPromotionValidator().ValidateMove(move, boardState) }}
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