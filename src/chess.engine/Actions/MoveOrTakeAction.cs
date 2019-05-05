using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class MoveOrTakeAction<TEntity> : BoardAction<TEntity> where TEntity : IBoardEntity
    {
        public MoveOrTakeAction(IBoardActionFactory<TEntity> factory, IBoardState<TEntity> boardState) : base(factory, boardState)
        {
        }
        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.To))
            {
                Factory.Create(DefaultActions.MoveOnly, BoardState).Execute(move);
            }
            else
            {
                Factory.Create(DefaultActions.TakeOnly, BoardState).Execute(move);
            }

        }
    }
}