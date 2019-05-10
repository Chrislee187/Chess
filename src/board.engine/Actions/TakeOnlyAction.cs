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
            BoardState.Remove(move.To);

            ActionProvider.Create((int)DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }
}