using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Actions
{
    public class CastleAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {
        public CastleAction(IBoardActionFactory<TEntity> factory, IBoardState<TEntity> boardState) : base(factory, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            BoardMove kingMove, rookMove;
            if (move.From.X < move.To.X) // King Side
            {
                kingMove = new BoardMove(move.From, BoardLocation.At($"G{move.From.Y}"), MoveType.MoveOnly);
                rookMove = new BoardMove(BoardLocation.At($"H{move.From.Y}"), BoardLocation.At($"F{move.From.Y}"), MoveType.MoveOnly);
            }
            else
            {
                kingMove = new BoardMove(move.From, BoardLocation.At($"C{move.From.Y}"), MoveType.MoveOnly);
                rookMove = new BoardMove(BoardLocation.At($"A{move.From.Y}"), BoardLocation.At($"D{move.From.Y}"), MoveType.MoveOnly);
            }

            var moveOnly = Factory.Create(DefaultActions.MoveOnly, BoardState);
            moveOnly.Execute(kingMove);
            moveOnly.Execute(rookMove);
        }
    }
}