using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Chess
{
    public class ChessGameState
    {
        // TODO: Needs tests
        public GameState CurrentGameState(IBoardState<ChessPieceEntity> boardState, Colours currentPlayer, Colours enemy)
        {
            var items = GetEnemiesAttackingKing(boardState, currentPlayer).ToList();
            return items.Any() 
                ? CheckForCheckMate(boardState, currentPlayer, items) 
                : GameState.InProgress;
        }

        // TODO: Pull out of board state, probably need some form of ChessGameState component we can passin!
        private IEnumerable<LocatedItem<ChessPieceEntity>> GetEnemiesAttackingKing(
            IBoardState<ChessPieceEntity> boardState, Colours kingColour)
        {
            var king = boardState.GetItems(kingColour, ChessPieceName.King).First();

            var locatedItems = boardState.GetItems(kingColour.Enemy());

            var enemiesAttackingKing = locatedItems.Where(itm
                => itm.Paths.ContainsMoveTo(king.Location));
            return enemiesAttackingKing;
        }

        private GameState CheckForCheckMate(IBoardState<ChessPieceEntity> boardState, Colours forPlayer,
            IEnumerable<LocatedItem<ChessPieceEntity>> enemiesAttackingKing)
        {
            // TODO: Pull this out to test
            var state = GameState.Check;
            var king = boardState.GetItems(forPlayer, ChessPieceName.King).Single();
            var kingCannotMove = !king.Paths.Any(); // Move validator will ensure we can't move into check

            var friendlyDestinations = boardState.GetAllMoveDestinations(forPlayer);

            var canBlock = enemiesAttackingKing.All(enemy =>
            {
                var attackingPath = enemy.Paths
                    .Single(attackPath => attackPath.Any(p => p.To.Equals(king.Location)));

                // Check if any friendly pieces can move to the path or take the item
                return friendlyDestinations.Any(fd => fd.Equals(enemy.Location)
                                                      || attackingPath.Any(move => move.To.Equals(fd))
                );
            });

            if (kingCannotMove && !canBlock)
            {
                state = GameState.Checkmate;
            }

            return state;
        }
    }
}