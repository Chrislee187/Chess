using System.Linq;
using chess.engine.Board;
using chess.engine.Game;

namespace chess.engine.Movement.Validators
{
    public class DestinationNotUnderAttackValidator<TEntity> : IMoveValidator<TEntity> 
        where TEntity : IBoardEntity
    {
        public bool ValidateMove(BoardMove move, IBoardState<TEntity> boardState)
        {
            var piece = boardState.GetItem(move.From);

            // TODO: enemy player logic shouldn't be here??? SHould we just force it two player black/white
            var playerColour = (Colours) piece.Item.Owner;
            var enemyColour = playerColour.Enemy();

            var enemyLocations = boardState.GetItems(enemyColour).Select(i => i.Location);

            var enemyPaths = new Paths();
            var enemyItems = boardState.GetItems(enemyLocations.ToArray());
            enemyPaths.AddRange(enemyItems.SelectMany(li => li.Paths));

            return !enemyPaths.ContainsMoveTo(move.To);

        }
    }
}