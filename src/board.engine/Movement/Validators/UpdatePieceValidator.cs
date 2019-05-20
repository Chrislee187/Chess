using System.ComponentModel.DataAnnotations;
using board.engine.Actions;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class UpdatePieceValidator<TEntity>
        : IMoveValidator<TEntity>
        where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<TEntity> roBoardState)
        {
            var piece = roBoardState.GetItem(move.From);
            if (piece == null) return false;

            bool valid = false;
            if(move.MoveType == (int) DefaultActions.UpdatePieceWithTake)
            {
                valid = new DestinationContainsEnemyMoveValidator<TEntity>()
                    .ValidateMove(move, roBoardState);
            }
            else
            {
                valid = new DestinationIsEmptyValidator<TEntity>()
                    .ValidateMove(move, roBoardState);
            }

            return valid;
        }
    }
}

