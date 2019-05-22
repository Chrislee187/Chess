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

        (bool result, LocatedItem<ChessPieceEntity> attacker) IsLocationUnderAttack(IBoardState<ChessPieceEntity> boardState,
            BoardLocation location, Colours defender);
    }

    // TODO: Badly need unit tests for this one.
    public class PlayerStateService : IPlayerStateService
    {
        private ILogger<IPlayerStateService> _logger;
        private readonly IFindAttackPaths _pathFinder;
        private readonly IPathsValidator<ChessPieceEntity> _pathsValidator;

        public PlayerStateService(ILogger<IPlayerStateService> logger, 
            IFindAttackPaths findAttackPaths,
            IPathsValidator<ChessPieceEntity> pathsValidator)
        {
            _logger = logger;
            _pathFinder = findAttackPaths;
            _pathsValidator = pathsValidator;
        }

        // TODO: Needs tests
        public PlayerState CurrentPlayerState(
            IBoardState<ChessPieceEntity> boardState, 
            Colours currentPlayer
            )
        {
            var king = boardState.GetItems((int)currentPlayer, (int)ChessPieceName.King).Single();

            var isLocationUnderAttack = IsLocationUnderAttack(boardState, king.Location, king.Item.Player);
            if(isLocationUnderAttack.result)
            {
                return CheckForCheckMate(boardState, king, isLocationUnderAttack.attacker);
            }


            return PlayerState.None;
        }

        public (bool result, LocatedItem<ChessPieceEntity> attacker) IsLocationUnderAttack(IBoardState<ChessPieceEntity> boardState,
            BoardLocation location, Colours defender)
        {
            var attackPaths = _pathFinder.Attacking(location, defender);

            var straightAttackPieces = new[] { ChessPieceName.Rook, ChessPieceName.Queen };
            var diagonalAttackPieces = new[] { ChessPieceName.Bishop, ChessPieceName.Queen };
            var knightAttackPieces = new[] { ChessPieceName.Knight };
            var pawnAttackPieces = new[] { ChessPieceName.Pawn };

            var attackPath = PathsContainsAttack(attackPaths.Straight, straightAttackPieces, defender.Enemy(), boardState);
            if (attackPath.result) return (true, attackPath.attacker);

            attackPath = PathsContainsAttack(attackPaths.Diagonal, diagonalAttackPieces, defender.Enemy(), boardState);
            if (attackPath.result) return (true, attackPath.attacker);

            attackPath = PathsContainsAttack(attackPaths.Knight, knightAttackPieces, defender.Enemy(), boardState);
            if (attackPath.result) return (true, attackPath.attacker);

            attackPath = PathsContainsAttack(attackPaths.Pawns, pawnAttackPieces, defender.Enemy(), boardState);
            if (attackPath.result) return (true, attackPath.attacker);

            return (false, null);
        }

        private static (bool result, LocatedItem<ChessPieceEntity> attacker) PathsContainsAttack(Paths paths,
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
                            return (true, piece);
                        }

                        break;
                    }
                }
            }

            return (false, null);
        }

        private void RefreshPiecePaths(IBoardState<ChessPieceEntity> boardState, LocatedItem<ChessPieceEntity> piece)
        {
            var validatedPaths = _pathsValidator.GetValidatedPaths(boardState, piece.Item, piece.Location);
            piece.UpdatePaths(validatedPaths);
        }

        private PlayerState CheckForCheckMate(IBoardState<ChessPieceEntity> boardState,
            LocatedItem<ChessPieceEntity> king, LocatedItem<ChessPieceEntity> attacker)
        {
            var state = PlayerState.Check;

            var clone = (IBoardState<ChessPieceEntity>) boardState.Clone();

             // Find the attacking piece
             // Not sure why we need to refresh it piece paths here but I get problems with out it
             // attcking piece
             RefreshPiecePaths(clone, attacker);

            // find the path it's attacking on
            var attackPath = attacker.Paths.FirstOrDefault(p => p.ContainsTo(king.Location));

            //
            // get all friendly pieces except king
            // refresh there paths
            var friendlyPieces = clone.GetItems(king.Item.Owner)
                .Where(p => p.Item.Piece != ChessPieceName.King)
                .OrderBy(p => p.Item.EntityType);

            // Remove the king from the board so as to not have it's location block 
            // attack paths from the new location
            clone.Remove(king.Location); 
            var kingCannotMove = !king.Paths.Any() || king.Paths.FlattenMoves()
                 .ToList()
                 .All(m 
                    => IsLocationUnderAttack(clone, m.To, king.Item.Player).result);


            // check if any friendly paths intersect the attackPath or attack the attacker
            var canBlock = friendlyPieces.Where(fp 
                => fp.Paths.FlattenMoves()
                    .Any(fm => attackPath
                        .Any(am => am.To.Equals(fm.To) || am.From.Equals(fm.To)
                        )
                    )
                );

            if (kingCannotMove && !canBlock.Any())
            {
                return PlayerState.Checkmate;
            }

            return PlayerState.Check;
        }
    }
}