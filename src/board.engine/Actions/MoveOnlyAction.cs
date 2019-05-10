using board.engine.Board;
using board.engine.Movement;

namespace board.engine.Actions
{
    public class MoveOnlyAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {
        public MoveOnlyAction(IBoardActionProvider<TEntity> actionProvider, IBoardState<TEntity> boardState) : base(actionProvider, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.From)) return;

            var piece = BoardState.GetItem(move.From).Item;
            BoardState.Remove(move.From);
            BoardState.PlaceEntity(move.To, piece);
        }
    }
}