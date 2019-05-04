using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class TakeOnlyAction<TEntity> : BoardAction<TEntity> where TEntity : IBoardEntity
    {

        public TakeOnlyAction(IBoardActionFactory<TEntity> factory, IBoardState<TEntity> boardState) : base(factory, boardState)
        {
        }
        public override void Execute(BoardMove move)
        {
            BoardState.Remove(move.To);

            Factory.Create(DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }
}