using board.engine.Board;
using board.engine.Movement;

namespace board.engine.Actions
{
    public class TakeOnlyAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {

        public TakeOnlyAction(IBoardActionFactory<TEntity> actionFactory, IBoardState<TEntity> boardState) : base(actionFactory, boardState)
        {
        }
        public override void Execute(BoardMove move)
        {
            BoardState.Remove(move.To);

            ActionFactory.Create((int)DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }
}