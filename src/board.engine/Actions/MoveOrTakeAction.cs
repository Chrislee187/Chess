using board.engine.Board;
using board.engine.Movement;

namespace board.engine.Actions
{
    public class MoveOrTakeAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {
        public MoveOrTakeAction(
            IBoardActionProvider<TEntity> actionProvider, 
            IBoardState<TEntity> boardState) 
            : base(actionProvider, boardState)
        {
        }
        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.To))
            {
                ActionProvider.Create((int)DefaultActions.MoveOnly, BoardState).Execute(move);
            }
            else
            {
                ActionProvider.Create((int)DefaultActions.TakeOnly, BoardState).Execute(move);
            }

        }
    }
}