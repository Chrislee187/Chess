using board.engine.Board;
using board.engine.Movement;

namespace board.engine.Actions
{
    public class MoveOrTakeAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {
        public MoveOrTakeAction(
            IBoardActionFactory<TEntity> actionFactory, 
            IBoardState<TEntity> boardState) 
            : base(actionFactory, boardState)
        {
        }
        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.To))
            {
                ActionFactory.Create((int)DefaultActions.MoveOnly, BoardState).Execute(move);
            }
            else
            {
                ActionFactory.Create((int)DefaultActions.TakeOnly, BoardState).Execute(move);
            }

        }
    }
}