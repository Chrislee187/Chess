using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using chess.engine.Board;
using chess.engine.Chess.Movement.ChessPieces.King;
using chess.engine.Chess.Movement.ChessPieces.Pawn;
using chess.engine.Entities;
using chess.engine.Movement.Validators;

namespace chess.engine.Movement
{
    public delegate bool BoardMovePredicate<TEntity>(BoardMove move, IBoardState<TEntity> boardState) 
        where TEntity : class, IBoardEntity;

    public interface IMoveValidationFactory<TEntity> 
        : IReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate<TEntity>>>
        where TEntity : class, IBoardEntity
    {

    }
    public class MoveValidationFactory<TEntity> 
        : ReadOnlyDictionary<MoveType, IEnumerable<BoardMovePredicate<TEntity>>>
            , IMoveValidationFactory<TEntity>
        where TEntity : class, IBoardEntity
    {
        
        public MoveValidationFactory() : base(
            new Dictionary<MoveType, IEnumerable<BoardMovePredicate<TEntity>>>
            {
                // Generic move types
                { MoveType.MoveOnly, new BoardMovePredicate<TEntity>[] {(move, boardState) => new DestinationIsEmptyValidator<TEntity>().ValidateMove(move, boardState) }},
                { MoveType.MoveOrTake, new BoardMovePredicate<TEntity>[] {(move, boardState) => new DestinationIsEmptyOrContainsEnemyValidator<TEntity>().ValidateMove(move, boardState)}},
                { MoveType.TakeOnly, new BoardMovePredicate<TEntity>[] {(move, boardState) => new DestinationContainsEnemyMoveValidator<TEntity>().ValidateMove(move, boardState) }},
                { MoveType.UpdatePiece, new BoardMovePredicate<TEntity>[] { (move, boardState) => new UpdatePieceValidator<TEntity>().ValidateMove(move, boardState) }},

                // TODO: Chess Move types shouldn't be here
                { MoveType.KingMove, new BoardMovePredicate<TEntity>[] {
                    (move, boardState) => new DestinationIsEmptyOrContainsEnemyValidator<TEntity>().ValidateMove(move, boardState),
                    (move, boardState) => new DestinationNotUnderAttackValidator<TEntity>().ValidateMove(move, boardState)}},
                { MoveType.TakeEnPassant, new BoardMovePredicate<TEntity>[] {(move, boardState) => new EnPassantTakeValidator().ValidateMove(move, (IBoardState<ChessPieceEntity>) boardState) }},
                { MoveType.CastleKingSide, new BoardMovePredicate<TEntity>[] { (move, boardState) => new KingCastleValidator().ValidateMove(move, (IBoardState<ChessPieceEntity>) boardState)  }},
                { MoveType.CastleQueenSide, new BoardMovePredicate<TEntity>[] { (move, boardState) => new KingCastleValidator().ValidateMove(move, (IBoardState<ChessPieceEntity>) boardState) }},
            })
        {}

        public IEnumerable<BoardMovePredicate<TEntity>> Create(MoveType moveType, IBoardState<TEntity> boardState)
        {
            if (ContainsKey(moveType))
            {
                return this[moveType];
            }

            throw new NotImplementedException($"MoveType: {moveType} not implemented");
        }
    }
}