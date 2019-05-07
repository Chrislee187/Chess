using System.Linq;
using chess.engine.Board;

namespace chess.engine.Movement.Validators
{
    public class DestinationNotUnderAttackValidator<TEntity> : IMoveValidator<TEntity> 
        where TEntity : class, IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            var piece = boardState.GetItem(move.From);

            var owner = piece.Item.Owner;

            var enemyLocations = boardState.GetAllItems().Where(i => i.Item.Owner != owner).Select(i => i.Location);

            var enemyPaths = new Paths();
            var enemyItems = boardState.GetItems(enemyLocations.ToArray());
            enemyPaths.AddRange(enemyItems.SelectMany(li => li.Paths));

            return !enemyPaths.ContainsMoveTo(move.To);

        }
    }
}