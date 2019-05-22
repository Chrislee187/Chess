using System.Collections.Generic;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Movement;
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
        private IFindAttackPaths _pathFinder;

        public PlayerStateService(ILogger<IPlayerStateService> logger, IFindAttackPaths findAttackPaths)
        {
            _logger = logger;
            _pathFinder = findAttackPaths;


        }

        // TODO: Needs tests
        public PlayerState CurrentPlayerState(
            IBoardState<ChessPieceEntity> boardState, 
            Colours currentPlayer
            )
        {
            var king = boardState.GetItems((int)currentPlayer, (int)ChessPieceName.King).Single();

            var kingState = IsLocationUnderAttack(boardState, king.Location, king.Item.Player)
                ? PlayerState.Check
                : PlayerState.None;
            if (kingState == PlayerState.None) return kingState;

           var enemies = boardState.GetItems((int) currentPlayer.Enemy())
                .ThatCanMoveTo(king.Location).ToList();

            return enemies.Any()
                ? CheckForCheckMate(boardState, enemies, king)
                : PlayerState.None;
        }

        private bool IsLocationUnderAttack(IBoardState<ChessPieceEntity> boardState,
            BoardLocation location, Colours defender)
        {
            var attackPaths = _pathFinder.Attacking(location, defender);

            var straightAttackPieces = new[] { ChessPieceName.Rook, ChessPieceName.Queen };
            var diagonalAttackPieces = new[] { ChessPieceName.Bishop, ChessPieceName.Queen };
            var knightAttackPieces = new[] { ChessPieceName.Knight };
            var pawnAttackPieces = new[] { ChessPieceName.Pawn };

            if (PathsContainsAttack(attackPaths.Straight, straightAttackPieces, defender.Enemy(), boardState)) return true;
            if (PathsContainsAttack(attackPaths.Diagonal, diagonalAttackPieces, defender.Enemy(), boardState)) return true;
            if (PathsContainsAttack(attackPaths.Knight, knightAttackPieces, defender.Enemy(), boardState)) return true;
            if (PathsContainsAttack(attackPaths.Pawns, pawnAttackPieces, defender.Enemy(), boardState)) return true;

            return false;
        }

        private static bool PathsContainsAttack(Paths paths,
            ChessPieceName[] straightAttackPieces, Colours enemy, IBoardState<ChessPieceEntity> boardState)
        {
            foreach (var attackPath in paths)
            {
                foreach (var path in attackPath)
                {
                    var piece = boardState.GetItem(path.To);
                    if (piece != null)
                    {
                        if (straightAttackPieces.Any(p => piece.Item.Is(enemy, p)))
                        {
                            return true;
                        }

                        break;
                    }
                }
            }

            return false;
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