using System;
using board.engine.Board;
using board.engine.Movement;

namespace board.engine.Actions
{
    public class TakeOnlyAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {

        public TakeOnlyAction(IBoardActionProvider<TEntity> actionProvider, IBoardState<TEntity> boardState) : base(actionProvider, boardState)
        {
        }
        public override void Execute(BoardMove move)
        {
            var takenItem = BoardState.GetItem(move.To);

            if(takenItem == null) throw new Exception("No piece found for TakeOnly action");

            BoardState.Remove(move.To);

            ActionProvider.Create((int)DefaultActions.MoveOnly, BoardState).Execute(move);

        }
    }
}