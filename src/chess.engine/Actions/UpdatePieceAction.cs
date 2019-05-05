using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Actions
{
    public class UpdatePieceAction<TEntity> : BoardAction<TEntity>
        where TEntity : class, IBoardEntity
    {
        public UpdatePieceAction(IBoardActionFactory<TEntity> factory, IBoardState<TEntity> boardState) : base(factory, boardState)
        {
        }

        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.From)) return;

            TEntity piece = BoardState.GetItem(move.From).Item;
            object forPlayer = piece.Owner;

            BoardState.Remove(move.From);

            if (!BoardState.IsEmpty(move.To))
            {
                BoardState.Remove(move.To);
            }
            // TODO: EntityFactory needs abstracting
            var chessPieceEntity = ChessPieceEntityFactory.Create((ChessPieceName)move.UpdateEntityType, (Colours) forPlayer);

            BoardState.PlaceEntity(move.To, chessPieceEntity as TEntity);
        }
    }
}