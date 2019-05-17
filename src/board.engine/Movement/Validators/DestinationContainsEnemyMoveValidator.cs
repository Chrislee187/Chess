using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationContainsEnemyMoveValidator<TEntity>
        : IMoveValidator<DestinationContainsEnemyMoveValidator<TEntity>.IBoardStateWrapper>
        where TEntity : class, IBoardEntity
    {
        public static IBoardStateWrapper Wrap(IBoardState<TEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper)
        {
            var sourcePiece = wrapper.GetFromEntity(move);
            if (sourcePiece == null) return false;

            var destinationPiece = wrapper.GetToEntity(move);
            if (destinationPiece == null) return false;

            return sourcePiece.Item.Owner != destinationPiece.Item.Owner;
        }

        public interface IBoardStateWrapper
        {
            LocatedItem<TEntity> GetFromEntity(BoardMove move);
            LocatedItem<TEntity> GetToEntity(BoardMove move);
        }

        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState) { }
        }
    }
}