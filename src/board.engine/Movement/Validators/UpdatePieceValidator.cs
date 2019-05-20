using board.engine.Actions;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class UpdatePieceValidator<TEntity>
        : IMoveValidator<UpdatePieceValidator<TEntity>.IBoardStateWrapper>
        where TEntity : class, IBoardEntity
    {
        public static IBoardStateWrapper Wrap(IBoardState<TEntity> boardState) => new BoardStateWrapper(boardState);

        public bool ValidateMove(BoardMove move, IBoardStateWrapper wrapper)
        {
            var piece = wrapper.GetFromEntity(move);
            if (piece == null) return false;

            var subWrapper = wrapper.GetDestinationIsEmptyOrContainsEnemyWrapper();


            var valid = new DestinationIsEmptyOrContainsEnemyValidator<TEntity>()
                .ValidateMove(move, subWrapper);

            return valid;
        }

        public interface IBoardStateWrapper
        {
            LocatedItem<TEntity> GetFromEntity(BoardMove move);

            DestinationIsEmptyOrContainsEnemyValidator<TEntity>.IBoardStateWrapper
                GetDestinationIsEmptyOrContainsEnemyWrapper();
        }

        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState)
            {
            }
        }
    }
}