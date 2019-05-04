using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using chess.engine.Board;
using chess.engine.Movement.ChessPieces.King;
using chess.engine.Movement.ChessPieces.Pawn;
using chess.engine.Movement.SimpleValidators;

namespace chess.engine.Movement
{
    public delegate bool BoardMovePredicate(BoardMove move, IBoardState boardState);

    public class MoveValidationFactory : ReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate>>
    {
        
        public MoveValidationFactory() : base(new Dictionary<MoveType, IEnumerable<BoardMovePredicate>>
        {
            // Generic move types
            { MoveType.MoveOnly, new BoardMovePredicate[] {(move, boardState) => new DestinationIsEmptyValidator().ValidateMove(move, boardState) }},
            { MoveType.MoveOrTake, new BoardMovePredicate[] {(move, boardState) => new DestinationIsEmptyOrContainsEnemyValidator().ValidateMove(move, boardState)}},
            { MoveType.TakeOnly, new BoardMovePredicate[] {(move, boardState) => new DestinationContainsEnemyMoveValidator().ValidateMove(move, boardState) }},
            { MoveType.UpdatePiece, new BoardMovePredicate[] { (move, boardState) => new UpdatePieceValidator().ValidateMove(move, boardState) }},

            // TODO: Chess Move types shouldn't be here
            { MoveType.KingMove, new BoardMovePredicate[] {
                (move, boardState) => new DestinationIsEmptyValidator().ValidateMove(move, boardState),
                (move, boardState) => new DestinationNotUnderAttackValidator().ValidateMove(move, boardState)}},
            { MoveType.TakeEnPassant, new BoardMovePredicate[] {(move, boardState) => new EnPassantTakeValidator().ValidateMove(move, boardState) }},
            { MoveType.CastleKingSide, new BoardMovePredicate[] { (move, boardState) => new KingCastleValidator().ValidateMove(move, boardState)  }},
            { MoveType.CastleQueenSide, new BoardMovePredicate[] { (move, boardState) => new KingCastleValidator().ValidateMove(move, boardState) }},
        })
        {}

        public IEnumerable<BoardMovePredicate> Create(MoveType moveType, IBoardState boardState)
        {
            if (ContainsKey(moveType))
            {
                return this[moveType];
            }

            throw new NotImplementedException($"MoveType: {moveType} not implemented");
        }
    }
}