using System;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationIsEmptyValidator<TEntity> : IMoveValidator<TEntity, DestinationIsEmptyValidator<TEntity>.IBoardStateWrapper>
        where TEntity : class, IBoardEntity
    {
        public static IBoardStateWrapper Wrap(IBoardState<TEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
            => throw new NotImplementedException(); 

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper) 
            => wrapper.GetToEntity(move) == null;

        public interface IBoardStateWrapper
        {
            LocatedItem<TEntity> GetToEntity(BoardMove move);
        }

        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState)
            {

            }
        }
    }

    public abstract class DefaultBoardStateWrapper<TEntity> where TEntity : class, IBoardEntity
    {
        protected IBoardState<TEntity> BoardState;

        protected DefaultBoardStateWrapper(IBoardState<TEntity> boardState)
            => BoardState = boardState;

        public LocatedItem<TEntity> GetFromEntity(BoardMove move)
        {
            return BoardState.GetFromItem(move);
        }

        public LocatedItem<TEntity> GetToEntity(BoardMove move)
        {
            return BoardState.GetToItem(move);
        }

        public DestinationIsEmptyOrContainsEnemyValidator<TEntity>.IBoardStateWrapper GetDestinationIsEmptyOrContainsEnemyWrapper() 
            => new DestinationIsEmptyOrContainsEnemyValidator<TEntity>.BoardStateWrapper(BoardState);

        public DestinationIsEmptyValidator<TEntity>.IBoardStateWrapper GetDestinationIsEmptyWrapper() 
            => new DestinationIsEmptyValidator<TEntity>.BoardStateWrapper(BoardState);

        public DestinationContainsEnemyMoveValidator<TEntity>.IBoardStateWrapper GetDestinationContainsEnemyMoveWrapper() 
            => new DestinationContainsEnemyMoveValidator<TEntity>.BoardStateWrapper(BoardState);
        public DestinationNotUnderAttackValidator<TEntity>.IBoardStateWrapper GetDestinationNotUnderAttackWrapper()
            => new DestinationNotUnderAttackValidator<TEntity>.BoardStateWrapper(BoardState);
    }
}