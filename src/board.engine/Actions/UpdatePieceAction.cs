using board.engine.Board;
using board.engine.Movement;

namespace board.engine.Actions
{
    public class UpdatePieceAction<TEntity> : BoardAction<TEntity>
        where TEntity : class, IBoardEntity
    {
        private readonly IBoardEntityFactory<TEntity> _entityFactory;

        public UpdatePieceAction(
            IBoardEntityFactory<TEntity> entityFactory,
            IBoardActionProvider<TEntity> actionProvider, 
            IBoardState<TEntity> boardState
            ) : base(actionProvider, boardState)
        {
            _entityFactory = entityFactory;
        }

        public override void Execute(BoardMove move)
        {
            if (BoardState.IsEmpty(move.From)) return;

//            var piece = BoardState.GetItem(move.From).Item;

            BoardState.Remove(move.From);

            if (!BoardState.IsEmpty(move.To))
            {
                BoardState.Remove(move.To);
            }

            var chessPieceEntity = _entityFactory.Create(move.ExtraData);

            BoardState.PlaceEntity(move.To, chessPieceEntity);
            chessPieceEntity.AddMoveTo(move.To);

        }
    }
}