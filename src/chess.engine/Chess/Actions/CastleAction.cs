using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Extensions;

namespace chess.engine.Chess.Actions
{
    public class CastleAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {
        public CastleAction(IBoardActionProvider<TEntity> actionProvider, IBoardState<TEntity> boardState) 
            : base(actionProvider, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            BoardMove kingMove, rookMove;
            if (move.From.X < move.To.X) // King Side
            {
                kingMove = new BoardMove(move.From, $"G{move.From.Y}".ToBoardLocation(), (int) DefaultActions.MoveOnly);
                rookMove = new BoardMove($"H{move.From.Y}".ToBoardLocation(), $"F{move.From.Y}".ToBoardLocation(), (int)DefaultActions.MoveOnly);
            }
            else
            {
                kingMove = new BoardMove(move.From, $"C{move.From.Y}".ToBoardLocation(), (int)DefaultActions.MoveOnly);
                rookMove = new BoardMove($"A{move.From.Y}".ToBoardLocation(), $"D{move.From.Y}".ToBoardLocation(), (int)DefaultActions.MoveOnly);
            }

            var moveOnly = ActionProvider.Create((int) DefaultActions.MoveOnly, BoardState);
            moveOnly.Execute(kingMove);
            moveOnly.Execute(rookMove);
        }
    }
}