using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Chess.Actions
{
    public class EnPassantAction : BoardAction
    {
        public EnPassantAction(IBoardActionFactory factory, IBoardState boardState) : base(factory, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.From)) return;

            var piece = BoardState.GetItem(move.From).Item;
            var passedPieceLoc = move.To.MoveBack(piece.Owner);

            BoardState.Remove(passedPieceLoc);
            Factory.Create(DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }
}