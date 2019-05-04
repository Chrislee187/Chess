using System;
using chess.engine.Board;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class FakeAction : BoardAction
    {
        public FakeAction(BoardActionFactory factory, IBoardState boardState) : base(factory, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            throw new NotImplementedException();
        }
    }
}