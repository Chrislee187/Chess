using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class MoveOrTakeAction : BoardAction
    {

        public MoveOrTakeAction(IBoardActionFactory factory, IBoardState boardState) : base(factory, boardState)
        {
        }
        public override void Execute(ChessMove move)
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