using System;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class FakeAction : BoardAction
    {
        public FakeAction(IBoardState state, BoardActionFactory factory) : base(state, factory)
        {
        }

        public override void Execute(ChessMove move)
        {
            throw new NotImplementedException();
        }
    }
}