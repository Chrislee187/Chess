using System.ComponentModel.DataAnnotations;
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

            bool valid = false;
            if(move.MoveType == (int) DefaultActions.UpdatePieceWithTake)
            {
                valid = new DestinationContainsEnemyMoveValidator<TEntity>()
                    .ValidateMove(move, wrapper.GetDestinationContainsEnemyMoveWrapper());
            }
            else
            {
                valid = new DestinationIsEmptyValidator<TEntity>()
                    .ValidateMove(move, wrapper.GetDestinationIsEmptyWrapper());
            }

            return valid;
        }

        public interface IBoardStateWrapper
        {
            LocatedItem<TEntity> GetFromEntity(BoardMove move);

            DestinationIsEmptyValidator<TEntity>.IBoardStateWrapper GetDestinationIsEmptyWrapper();
            DestinationContainsEnemyMoveValidator<TEntity>.IBoardStateWrapper GetDestinationContainsEnemyMoveWrapper();
        }

        public class BoardStateWrapper : DefaultBoardStateWrapper<TEntity>, IBoardStateWrapper
        {
            public BoardStateWrapper(IBoardState<TEntity> boardState) : base(boardState)
            {
            }
        }
    }
}

