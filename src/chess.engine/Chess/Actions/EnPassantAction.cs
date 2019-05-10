using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess.Movement.ChessPieces.King;
using chess.engine.Game;

namespace chess.engine.Chess.Actions
{
    public class EnPassantAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {
        public EnPassantAction(
            IBoardActionProvider<TEntity> provider, 
            IBoardState<TEntity> boardState
            ) : base(provider, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.From)) return;

            var piece = BoardState.GetItem(move.From).Item;

            var passedPieceLoc = move.To.MoveBack((Colours) piece.Owner);

            BoardState.Remove(passedPieceLoc);
            ActionProvider.Create((int) DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }
}