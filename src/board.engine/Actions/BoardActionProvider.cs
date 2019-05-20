using System;
using System.Collections.Generic;
using board.engine.Board;

namespace board.engine.Actions
{
    public interface IBoardActionProvider<TEntity> where TEntity : class, IBoardEntity
    {
        IBoardAction Create(int action, IBoardState<TEntity> boardState);
    }

    public abstract class BoardActionProvider<TEntity> : IBoardActionProvider<TEntity> where TEntity : class, IBoardEntity

    {
        protected readonly Dictionary<int, Func<IBoardState<TEntity>, IBoardAction>> Actions;

        public BoardActionProvider(IBoardEntityFactory<TEntity> entityFactory)
        {
            Actions = new Dictionary<int, Func<IBoardState<TEntity>, IBoardAction>>
            {
                {(int) DefaultActions.MoveOnly, (s) => new MoveOnlyAction<TEntity>(this, s)},
                {(int) DefaultActions.TakeOnly, (s) => new TakeOnlyAction<TEntity>(this, s)},
                {(int) DefaultActions.MoveOrTake, (s) => new MoveOrTakeAction<TEntity>(this, s)},
                {(int) DefaultActions.UpdatePiece, (s) => new UpdatePieceAction<TEntity>(entityFactory, this, s)},
                {(int) DefaultActions.UpdatePieceWithTake, (s) => new UpdatePieceAction<TEntity>(entityFactory, this, s)},
            };
        }

        public IBoardAction Create(int moveType, IBoardState<TEntity> boardState)
        {
            if (Actions.ContainsKey(moveType))
            {
                return Actions[moveType](boardState);
            }

            throw new NotImplementedException($"Action: {moveType} not implemented");
        }
    }

    public enum DefaultActions
    {
        // General moves, not chess specific
        MoveOrTake = 1,
        MoveOnly = 2,
        TakeOnly = 3,
        UpdatePiece = 4,
        UpdatePieceWithTake = 5,
    }
}