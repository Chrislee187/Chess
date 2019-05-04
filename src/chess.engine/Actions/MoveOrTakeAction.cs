using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class MoveOrTakeAction<TEntity> : BoardAction<TEntity>
        where TEntity : class, IBoardEntity
    {

        public MoveOrTakeAction(IBoardActionFactory<TEntity> factory, IBoardState<TEntity> boardState) : base(factory, boardState)
        {
        }
        public override void Execute(BoardMove move)
        {
            var dest = BoardState.GetItem(move.To)?.Item;

            if (dest != null)
            {
                Factory.Create(DefaultActions.TakeOnly, BoardState).Execute(move);
            }
            else
            {
                Factory.Create(DefaultActions.MoveOnly, BoardState).Execute(move);
            }

        }
    }
}