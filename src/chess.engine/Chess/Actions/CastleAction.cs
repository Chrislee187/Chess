using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Actions
{
    public class CastleAction : BoardAction
    {
        public CastleAction(IBoardActionFactory factory, IBoardState boardState) : base(factory, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            BoardMove kingMove, rookMove;
            if (move.From.File < move.To.File) // King Side
            {
                kingMove = new BoardMove(move.From, BoardLocation.At($"G{move.From.Rank}"), MoveType.MoveOnly);
                rookMove = new BoardMove(BoardLocation.At($"H{move.From.Rank}"), BoardLocation.At($"F{move.From.Rank}"), MoveType.MoveOnly);
            }
            else
            {
                kingMove = new BoardMove(move.From, BoardLocation.At($"C{move.From.Rank}"), MoveType.MoveOnly);
                rookMove = new BoardMove(BoardLocation.At($"A{move.From.Rank}"), BoardLocation.At($"D{move.From.Rank}"), MoveType.MoveOnly);
            }

            var moveOnly = Factory.Create(DefaultActions.MoveOnly, BoardState);
            moveOnly.Execute(kingMove);
            moveOnly.Execute(rookMove);
        }
    }
}