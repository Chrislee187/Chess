using System.Linq;
using chess.engine.Board;
using chess.engine.Game;

namespace chess.engine.Movement.SimpleValidators
{
    public class DestinationNotUnderAttackValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, IBoardState boardState)
        {
            var movingPiece = boardState.GetItem(move.From);
            var enemyColour = movingPiece.Item.Player.Enemy();
            var enemyLocations = boardState.GetItems(enemyColour).Select(i => i.Location).ToList();

            var enemyPaths = new Paths();
            var locatedItems = boardState.GetItems(enemyLocations.ToArray()).ToList();
            var selectMany = locatedItems.SelectMany(li => li.Paths).ToList();
            enemyPaths.AddRange(selectMany);

            return !enemyPaths.FlattenMoves().Any(enemyMove=> enemyMove.To.Equals(move.To));

        }
    }
}