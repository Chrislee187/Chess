using System.Collections.Generic;
using System.Linq;
using board.engine.Board;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using Microsoft.Extensions.Logging;

namespace chess.engine.Chess
{
    public interface IChessGameStateService
    {
        GameState CurrentGameState(IBoardState<ChessPieceEntity> boardState, Colours currentPlayer);
    }

    public class ChessGameStateService : IChessGameStateService
    {
        private ILogger<ChessGameStateService> _logger;

        public ChessGameStateService(ILogger<ChessGameStateService> logger)
        {
            _logger = logger;
        }
        // TODO: Needs tests
        public GameState CurrentGameState(IBoardState<ChessPieceEntity> boardState, Colours currentPlayer)
        {
            var items = GetEnemiesAttackingKing(boardState, currentPlayer).ToList();
            return items.Any() 
                ? CheckForCheckMate(boardState, currentPlayer, items) 
                : GameState.InProgress;
        }

        private IEnumerable<LocatedItem<ChessPieceEntity>> GetEnemiesAttackingKing(
            IBoardState<ChessPieceEntity> boardState, Colours kingColour)
        {
            var king = boardState.GetItems((int) kingColour, (int) ChessPieceName.King).First();

            var locatedItems = boardState.GetItems((int) kingColour.Enemy());

            var enemiesAttackingKing = locatedItems.Where(itm
                => itm.Paths.ContainsMoveTo(king.Location));
            return enemiesAttackingKing;
        }

        private GameState CheckForCheckMate(IBoardState<ChessPieceEntity> boardState, Colours forPlayer,
            IEnumerable<LocatedItem<ChessPieceEntity>> enemiesAttackingKing)
        {
            // TODO: Pull this out to test
            var state = GameState.Check;
            var king = boardState.GetItems((int)forPlayer, (int)ChessPieceName.King).Single();
            var kingCannotMove = !king.Paths.Any(); // Move validator will ensure we can't move into check

            var friendlyDestinations = boardState.GetAllMoveDestinations((int) forPlayer);

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