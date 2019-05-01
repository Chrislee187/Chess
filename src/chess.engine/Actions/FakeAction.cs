using System;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class FakeAction : BoardAction
    {
        public FakeAction(IBoardState state) : base(state)
        {
        }

        public override void Execute(ChessMove move)
        {
            throw new NotImplementedException();
        }
    }
}