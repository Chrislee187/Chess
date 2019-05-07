using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess.Actions
{
    public class EnPassantAction<TEntity> : BoardAction<TEntity> where TEntity : class, IBoardEntity
    {
        public EnPassantAction(IBoardActionFactory<TEntity> factory, IBoardState<TEntity> boardState) : base(factory, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.From)) return;

            var piece = BoardState.GetItem(move.From).Item;

            var passedPieceLoc = move.To.MoveBack((Colours) piece.Owner);

            BoardState.Remove(passedPieceLoc);
            Factory.Create(DefaultActions.MoveOnly, BoardState).Execute(move);
        }
    }
}