using System;
using System.Collections.Generic;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement.Validators;

namespace board.engine.Movement
{
    public class MoveValidationProvider<TEntity> : IMoveValidationProvider<TEntity>
       where TEntity : class, IBoardEntity
    {
        protected static Dictionary<int, IEnumerable<BoardMovePredicate<TEntity>>> Validators;

        public MoveValidationProvider()
        {
            Validators = new Dictionary<int, IEnumerable<BoardMovePredicate<TEntity>>>
            {
                // Generic move types
                { (int) DefaultActions.MoveOnly, new BoardMovePredicate<TEntity>[] {(move, boardState) => new DestinationIsEmptyValidator<TEntity>().ValidateMove(move, boardState) }},
                { (int) DefaultActions.MoveOrTake, new BoardMovePredicate<TEntity>[] {(move, boardState) => new DestinationIsEmptyOrContainsEnemyValidator<TEntity>().ValidateMove(move, boardState)}},
                { (int) DefaultActions.TakeOnly, new BoardMovePredicate<TEntity>[] {(move, boardState) => new DestinationContainsEnemyMoveValidator<TEntity>().ValidateMove(move, boardState) }},
                { (int) DefaultActions.UpdatePiece, new BoardMovePredicate<TEntity>[] { (move, boardState) => new UpdatePieceValidator<TEntity>().ValidateMove(move, boardState) }},
            };

        }

        public IEnumerable<BoardMovePredicate<TEntity>> Create(int chessMoveTypes, IBoardState<TEntity> boardState)
        {
            if (Validators.ContainsKey(chessMoveTypes))
            {
                return Validators[chessMoveTypes];
            }

            throw new NotImplementedException($"MoveType: {chessMoveTypes} not implemented");
        }

        public bool TryGetValue(int moveType, out IEnumerable<BoardMovePredicate<TEntity>> validators)
        {
            return Validators.TryGetValue(moveType, out validators);
        }
    }

}