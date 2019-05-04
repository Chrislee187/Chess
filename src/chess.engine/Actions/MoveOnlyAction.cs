using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class MoveOnlyAction : BoardAction
    {
        public MoveOnlyAction(IBoardActionFactory factory, IBoardState boardState) : base(factory, boardState)
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