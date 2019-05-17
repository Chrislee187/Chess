using System;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationIsEmptyOrContainsEnemyValidator<TEntity> 
        : IMoveValidator<TEntity, DestinationIsEmptyOrContainsEnemyValidator<TEntity>.IBoardStateWrapper> 
        where TEntity : class, IBoardEntity
    {
        public static IBoardStateWrapper Wrap(IBoardState<TEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            throw new NotImplementedException();
//            var wrap1 = DestinationIsEmptyValidator<TEntity>.Wrap(boardState);
//            var wrap2 = DestinationContainsEnemyMoveValidator<TEntity>.Wrap(boardState);
//
//            return new DestinationIsEmptyValidator<TEntity>().ValidateMove(move, wrap1)
//                   || new DestinationContainsEnemyMoveValidator<TEntity>().ValidateMove(move, wrap2);
        }

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper)
        {
            return new DestinationIsEmptyValidator<TEntity>().ValidateMove(move, wrapper.GetDestinationIsEmptyWrapper())
                   || new DestinationContainsEnemyMoveValidator<TEntity>().ValidateMove(move, wrapper.GetDestinationContainsEnemyMoveWrapper());
        }

        public interface IBoardStateWrapper
        {
            DestinationIsEmptyValidator<TEntity>.IBoardStateWrapper
                GetDestinationIsEmptyWrapper();
            DestinationContainsEnemyMoveValidator<TEntity>.IBoardStateWrapper
                GetDestinationContainsEnemyMoveWrapper();
        }

        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState)
            {
            }
        }
    }
}