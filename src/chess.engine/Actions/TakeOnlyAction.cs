using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class TakeOnlyAction : BoardAction
    {

        public TakeOnlyAction(IBoardActionFactory factory, IBoardState boardState) : base(factory, boardState)
        {
        }
        public override void Execute(ChessMove move)
        {
            BoardState.Remove(move.To);

            Factory.Create(DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }
}