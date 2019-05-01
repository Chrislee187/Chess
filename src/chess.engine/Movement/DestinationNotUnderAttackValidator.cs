using System.Linq;
using chess.engine.Board;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public class DestinationNotUnderAttackValidator : IMoveValidator
    {
        public bool ValidateMove(ChessMove move, BoardState boardState)
        {
            var movingPiece = boardState.Entities[move.From];
            var enemyColour = movingPiece.Player.Enemy();
            var enemyLocations = boardState.Entities.Where(kvp => kvp.Value?.Player == enemyColour).Select(kvp => kvp.Key);

            var enemyPaths = boardState.Paths.Where(kvp => kvp.Value != null && enemyLocations.Contains(kvp.Key)).ToList();
            var flattenPaths = enemyPaths.SelectMany(kvp => kvp.Value).SelectMany(p => p).ToList();

            return !flattenPaths.Any(enemyMove
                => Equals(enemyMove.To, move.To));

        }
    }
}