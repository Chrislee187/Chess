using System;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Movement;
using Microsoft.Extensions.Logging;

namespace chess.engine.Game
{
    /// <summary>
    /// DO NOT USE IRefreshAllPaths in this service, causes endless reecursion on the path refresh mechanism
    /// which gets called when analysing a move to see if it causes "check"
    /// (the analysis, causes a board refresh, which re-generates the available moves, which has to in
    /// turn see if any of those moves cause "check" etc. etc.)
    /// </summary>
    public class CheckDetectionService : ICheckDetectionService
    {
        private readonly ILogger<CheckDetectionService> _logger;
        private readonly IPlayerStateService _playerStateService;
        private readonly IBoardMoveService<ChessPieceEntity> _moveService;
        private readonly IFindAttackPaths _pathFinder;
        private readonly IPathsValidator<ChessPieceEntity> _pathsValidator;

        public CheckDetectionService(
            ILogger<CheckDetectionService> logger,
            IPlayerStateService playerStateService,
            IBoardMoveService<ChessPieceEntity> moveService,
            IFindAttackPaths findAttackPaths,
            IPathsValidator<ChessPieceEntity> pathsValidator
        )
        {
            _logger = logger;
            _moveService = moveService;
            _playerStateService = playerStateService;
            _pathFinder = findAttackPaths;
            _pathsValidator = pathsValidator;
        }

        public GameCheckState Check(IBoardState<ChessPieceEntity> boardState)
        {
            var whiteState = _playerStateService.CurrentPlayerState(boardState, Colours.White);
            var blackState = _playerStateService.CurrentPlayerState(boardState, Colours.Black);
            if (whiteState == PlayerState.None && blackState == PlayerState.None)
            {
                return GameCheckState.None;
            }

            if (whiteState != PlayerState.None && blackState != PlayerState.None)
            {
                throw new Exception($"Invalid game states white/black {whiteState}/{blackState}");
            }

            if (blackState == PlayerState.None)
            {
                return whiteState == PlayerState.Checkmate
                    ? GameCheckState.WhiteCheckmated
                    : GameCheckState.WhiteInCheck;
            }

            return blackState == PlayerState.Checkmate
                ? GameCheckState.BlackCheckmated
                : GameCheckState.BlackInCheck;
        }

        // Used more specifically to enforce board logic
        public bool DoesMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move)
        {
            // NOTE: We could reuse the PlayerStateService here
            var attackingPlayer = boardState.GetItem(move.From).Item.Player;

            var clone = (IBoardState<ChessPieceEntity>) boardState.Clone();
            _moveService.Move(clone, move);

            // NOTE: Paths are NOT refreshed by the MoveService and we MUST NOT refresh alls paths here
            // typically already inside a refresh cycle when we perform this check (it would also slow things down a lot)
            var friendlyKing = clone.GetItems((int) attackingPlayer, (int) ChessPieceName.King).Single();

            var doesMoveLeaveUsInCheck = _playerStateService.IsLocationUnderAttack(clone, friendlyKing.Location, friendlyKing.Item.Player);
            return doesMoveLeaveUsInCheck.result;

// NOTE: 22/05/2019 - This is whats needed to use CurrentPlayerState() method on the PlayerStateService, the refreshes hammer performance
// Leave this around for a while as an alternative approach should we require it.
// 
//            RefreshPiecePaths(clone, friendlyKing);
//            var friendlyPieces = clone.GetItems((int) attackingPlayer.Enemy())
//                .Where(p => p.Item.Piece != ChessPieceName.King)
//                .OrderBy(p => p.Item.EntityType);
//
//            friendlyPieces.AsParallel()
//                .ForAll(piece => RefreshPiecePaths(clone, piece));
//
//            return _playerStateService.CurrentPlayerState(clone, attackingPlayer) != PlayerState.None;
        }

        // Typically only used to produce correct san notation on output
        public bool DoesMoveCauseCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move)
        {
            var attackingPlayer = boardState.GetItem(move.From).Item.Player;

            var defender = boardState.GetItem(move.To);
            if (defender != null && defender.Item.Is(attackingPlayer.Enemy(), ChessPieceName.King))
            {
                return true;
            }

            var clone = (IBoardState<ChessPieceEntity>)boardState.Clone();
            _moveService.Move(clone, move);
            RefreshPiecePaths(clone, clone.GetItem(move.To));

            var defendingKing = clone.GetItems((int)attackingPlayer.Enemy(), (int)ChessPieceName.King).Single();
            RefreshPiecePaths(clone, defendingKing);
            var friendlyPieces = clone.GetItems((int)attackingPlayer.Enemy())
                .Where(p => p.Item.Piece != ChessPieceName.King)
                .OrderBy(p => p.Item.EntityType);

            friendlyPieces.AsParallel()
                .ForAll(piece => RefreshPiecePaths(clone, piece));
            // NOTE: The PlayerStateService is dependent on valid piece paths to determine checkmate conditions
            // but we cannot refresh the board here (see the note in DoesMoveLeaveUsInCheck
            return _playerStateService.CurrentPlayerState(clone, attackingPlayer.Enemy()) != PlayerState.None;
//
//             var defendingKing = clone.GetItems((int)attackingPlayer.Enemy(), (int)ChessPieceName.King).Single();
//            return IsLocationUnderAttack(clone, defendingKing.Location, defendingKing.Item.Player);
        }
        private void RefreshPiecePaths(IBoardState<ChessPieceEntity> boardState, LocatedItem<ChessPieceEntity> piece)
        {
            var validatedPaths = _pathsValidator.GetValidatedPaths(boardState, piece.Item, piece.Location);
            piece.UpdatePaths(validatedPaths);
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

    }

    public interface ICheckDetectionService
    {
        GameCheckState Check(IBoardState<ChessPieceEntity> boardState);
        bool DoesMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move);
        bool DoesMoveCauseCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move);
    }

    public enum GameCheckState
    {
        None,
        WhiteInCheck,
        WhiteCheckmated,
        BlackInCheck,
        BlackCheckmated
    }
}