using System.Collections.Generic;
using System.Linq;
using board.engine.Board;
using chess.engine.Entities;
using Microsoft.Extensions.Logging;

namespace chess.engine.Game
{
    public interface IPlayerStateService
    {
        PlayerState CurrentPlayerState(IBoardState<ChessPieceEntity> boardState, Colours currentPlayer);
    }

    public class PlayerStateService : IPlayerStateService
    {
        private ILogger<IPlayerStateService> _logger;

        public PlayerStateService(ILogger<IPlayerStateService> logger)
        {
            _logger = logger;
        }

        // TODO: Needs tests
        public PlayerState CurrentPlayerState(
            IBoardState<ChessPieceEntity> boardState, 
            Colours currentPlayer
            )
        {
            var king = boardState.GetItems((int)currentPlayer, (int)ChessPieceName.King).Single();

            var enemies = boardState.GetItems((int) currentPlayer.Enemy())
                .ThatCanMoveTo(king.Location).ToList();

            return enemies.Any()
                ? CheckForCheckMate(boardState, enemies, king)
                : PlayerState.InProgress;
        }

        private PlayerState CheckForCheckMate(
            IBoardState<ChessPieceEntity> boardState,
            IEnumerable<LocatedItem<ChessPieceEntity>> enemiesAttackingKing, 
            LocatedItem<ChessPieceEntity> king)
        {
            var state = PlayerState.Check;
            var kingCannotMove = !king.Paths.Any(); // Move validator will ensure we can't move into check

            var friendlyDestinations = boardState.GetItems(king.Item.Owner)
                .AllDestinations();

            var canBlock = enemiesAttackingKing.All(enemy =>
            {
                // NOTE: Edge case here, a pawn has multiple attack paths to the same location for
                // each promotion piece when applicable, we only care about one of them here.
                var attackingPath = enemy.Paths
                    .First(attackPath => attackPath.CanMoveTo(king.Location));

                // Check if any friendly pieces can move to the path or take the item
                return friendlyDestinations.Any(fd => fd.Equals(enemy.Location)
                                                      || attackingPath.CanMoveTo(fd)
                                                      );
            });

            if (kingCannotMove && !canBlock)
            {
                state = PlayerState.Checkmate;
            }

            return state;
        }
    }
}