using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationIsEmptyValidator<TEntity> : IMoveValidator<DestinationIsEmptyValidator<TEntity>.IBoardStateWrapper>
        where TEntity : class, IBoardEntity
    {
        public static IBoardStateWrapper Wrap(IBoardState<TEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper) 
            => wrapper.GetToEntity(move) == null;

        public interface IBoardStateWrapper
        {
            LocatedItem<TEntity> GetToEntity(BoardMove move);
        }

        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState)
            {

            }
        }
    }
}