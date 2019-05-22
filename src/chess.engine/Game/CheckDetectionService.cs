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
            var attackingPlayer = boardState.GetItem(move.From).Item.Player;

            var clone = (IBoardState<ChessPieceEntity>) boardState.Clone();
            _moveService.Move(clone, move);

            var king = clone.GetItems((int) attackingPlayer, (int) ChessPieceName.King).Single();

            return _playerStateService.IsLocationUnderCheck(clone, king.Location, king.Item.Player).result;
        }

        // Typically only used to produce correct san notation on output
        public bool DoesMoveCauseCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move)
        {
            // TODO: Would it be quicker to 
            var attackingPlayer = boardState.GetItem(move.From).Item.Player;

            var defender = boardState.GetItem(move.To);
            if (defender != null && defender.Item.Is(attackingPlayer.Enemy(), ChessPieceName.King))
            {
                return true;
            }

            var clone = (IBoardState<ChessPieceEntity>)boardState.Clone();
            _moveService.Move(clone, move);

            var king = clone.GetItems((int)attackingPlayer.Enemy(), (int)ChessPieceName.King).Single();
            return _playerStateService.IsLocationUnderCheck(boardState, king.Location, king.Item.Player).result;
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