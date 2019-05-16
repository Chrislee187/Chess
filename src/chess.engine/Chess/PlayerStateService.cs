using System.Collections.Generic;
using System.Linq;
using board.engine;
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

    public class PlayerStateService : IChessGameStateService
    {
        private ILogger<PlayerStateService> _logger;

        public PlayerStateService(ILogger<PlayerStateService> logger)
        {
            _logger = logger;
        }

        // TODO: Needs tests
        public GameState CurrentGameState(
            IBoardState<ChessPieceEntity> boardState, 
            Colours currentPlayer
            )
        {
            var king = boardState.GetItems((int)currentPlayer, (int)ChessPieceName.King).Single();

            var enemies = boardState.GetItems((int) currentPlayer.Enemy())
                .ThatCanMoveTo(king.Location).ToList();

            return enemies.Any()
                ? CheckForCheckMate(boardState, enemies, king)
                : GameState.InProgress;
        }

        private GameState CheckForCheckMate(
            IBoardState<ChessPieceEntity> boardState,
            IEnumerable<LocatedItem<ChessPieceEntity>> enemiesAttackingKing, 
            LocatedItem<ChessPieceEntity> king)
        {
            var state = GameState.Check;
            var kingCannotMove = !king.Paths.Any(); // Move validator will ensure we can't move into check

            var friendlyDestinations = boardState.GetItems(king.Item.Owner)
                .AllDestinations();

            var canBlock = enemiesAttackingKing.All(enemy =>
            {
                // BUG: What if this returns more than one? Test this properly
                var attackingPath = enemy.Paths
                    .Single(attackPath => attackPath.CanMoveTo(king.Location));

                // Check if any friendly pieces can move to the path or take the item
                return friendlyDestinations.Any(fd => fd.Equals(enemy.Location)
                                                      || attackingPath.CanMoveTo(fd)
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