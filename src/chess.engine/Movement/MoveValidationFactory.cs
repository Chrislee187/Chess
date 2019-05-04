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

    public delegate bool ChessBoardMovePredicate(BoardMove move, IBoardState boardState);

    public interface IMoveValidator
    {
        bool ValidateMove(BoardMove move, IBoardState boardState);
    }

    public class MoveValidationFactory : ReadOnlyDictionary<MoveType, IEnumerable<ChessBoardMovePredicate>>
    {
        
        public MoveValidationFactory() : base(new Dictionary<MoveType, IEnumerable<ChessBoardMovePredicate>>
        {
            // Generic move types
            { MoveType.MoveOnly, new ChessBoardMovePredicate[] {(move, boardState) => new DestinationIsEmptyValidator().ValidateMove(move, boardState) }},
            { MoveType.MoveOrTake, new ChessBoardMovePredicate[] {(move, boardState) => new DestinationIsEmptyOrContainsEnemyValidator().ValidateMove(move, boardState)}},
            { MoveType.TakeOnly, new ChessBoardMovePredicate[] {(move, boardState) => new DestinationContainsEnemyMoveValidator().ValidateMove(move, boardState) }},
            { MoveType.UpdatePiece, new ChessBoardMovePredicate[] { (move, boardState) => new UpdatePieceValidator().ValidateMove(move, boardState) }},

            // TODO: Chess Move types shouldn't be here
            { MoveType.KingMove, new ChessBoardMovePredicate[] {
                (move, boardState) => new DestinationIsEmptyValidator().ValidateMove(move, boardState),
                (move, boardState) => new DestinationNotUnderAttackValidator().ValidateMove(move, boardState)}},
            { MoveType.TakeEnPassant, new ChessBoardMovePredicate[] {(move, boardState) => new EnPassantTakeValidator().ValidateMove(move, boardState) }},
            { MoveType.CastleKingSide, new ChessBoardMovePredicate[] { (move, boardState) => new KingCastleValidator().ValidateMove(move, boardState)  }},
            { MoveType.CastleQueenSide, new ChessBoardMovePredicate[] { (move, boardState) => new KingCastleValidator().ValidateMove(move, boardState) }},
        })
        {}

        public IEnumerable<ChessBoardMovePredicate> Create(MoveType moveType, IBoardState boardState)
        {
            if (ContainsKey(moveType))
            {
                return this[moveType];
            }

            throw new NotImplementedException($"MoveType: {moveType} not implemented");
        }
    }
}