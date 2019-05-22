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
    public class CheckDetectionService : ICheckDetectionService
    {
        private readonly ILogger<CheckDetectionService> _logger;
        private readonly IPlayerStateService _playerStateService;
        private readonly IBoardMoveService<ChessPieceEntity> _moveService;
        private readonly IFindAttackPaths _pathFinder;

        public CheckDetectionService(
            ILogger<CheckDetectionService> logger,
            IPlayerStateService playerStateService,
            IBoardMoveService<ChessPieceEntity> moveService,
            IFindAttackPaths findAttackPaths

        )
        {
            _logger = logger;
            _moveService = moveService;
            _playerStateService = playerStateService;
            _pathFinder = findAttackPaths;

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

            var whiteKing = boardState.GetItems((int)Colours.White, (int)ChessPieceName.King).Single();
            if (whiteState != PlayerState.None)
            {
                if (!whiteKing.Paths.Any()) return GameCheckState.WhiteCheckmated;
                return GameCheckState.WhiteInCheck;
            }

            var blackKing = boardState.GetItems((int)Colours.Black, (int)ChessPieceName.King).Single();
            if (blackState != PlayerState.None)
            {
                if (!blackKing.Paths.Any()) return GameCheckState.BlackCheckmated;
                return GameCheckState.BlackInCheck;
            }

            return GameCheckState.None;
        }

        public bool DoesMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move)
        {
            var defendingPlayer = boardState.GetItem(move.From).Item.Player;

            var clone = (IBoardState<ChessPieceEntity>) boardState.Clone();
            _moveService.Move(clone, move);

            var defendingKing = clone.GetItems((int)defendingPlayer, (int)ChessPieceName.King).Single();
            return IsLocationUnderAttack(clone, defendingKing.Location, defendingPlayer);
        }

        private bool IsLocationUnderAttack(IBoardState<ChessPieceEntity> boardState,
            BoardLocation location, Colours defender)
        {
            var attackPaths = _pathFinder.Attacking(location, defender);

            var straightAttackPieces = new[] {ChessPieceName.Rook, ChessPieceName.Queen};
            var diagonalAttackPieces = new[] {ChessPieceName.Bishop, ChessPieceName.Queen};
            var knightAttackPieces = new[] {ChessPieceName.Knight};
            var pawnAttackPieces = new[] {ChessPieceName.Pawn};

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
        bool DoesMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move);
        GameCheckState Check(IBoardState<ChessPieceEntity> boardState);
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