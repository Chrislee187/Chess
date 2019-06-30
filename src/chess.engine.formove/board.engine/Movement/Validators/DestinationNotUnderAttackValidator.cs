using System.Linq;
using board.engine.Actions;
using board.engine.Board;

namespace board.engine.Movement.Validators
{
    public class DestinationNotUnderAttackValidator<TEntity> 
        : IMoveValidator<TEntity> 
        where TEntity : class, IBoardEntity
    {

        public bool ValidateMove(BoardMove move, IReadOnlyBoardState<TEntity> roBoardState)
        {
            var piece = roBoardState.GetItem(move.From);
            if (piece == null) return false;

            var owner = piece.Item.Owner;

            var enemyPaths = new Paths();
            var enemyItems = roBoardState.GetItems().Where(i => i.Item.Owner != owner);
            enemyPaths.AddRange(enemyItems.SelectMany(li => li.Paths));

            var attackMoveTypes = new []{ (int) DefaultActions.MoveOrTake, (int) DefaultActions.TakeOnly, (int) ChessMoveTypes.KingMove };
            return !enemyPaths.ContainsMoveTypeTo(move.To, attackMoveTypes);
        }

    }
}