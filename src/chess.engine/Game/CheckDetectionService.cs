using System;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using Microsoft.Extensions.Logging;

namespace chess.engine.Game
{
    public class CheckDetectionService : ICheckDetectionService
    {
        private readonly ILogger<CheckDetectionService> _logger;
        private readonly IPlayerStateService _playerStateService;
        private readonly IBoardMoveService<ChessPieceEntity> _moveService;

        public CheckDetectionService(
            ILogger<CheckDetectionService> logger,
            IPlayerStateService playerStateService,
            IBoardMoveService<ChessPieceEntity> moveService
        )
        {
            _logger = logger;
            _moveService = moveService;
            _playerStateService = playerStateService;
        }

        public GameCheckState Check(IBoardState<ChessPieceEntity> boardState)
        {
            var clonedBoardState = (IBoardState<ChessPieceEntity>) boardState.Clone();

            var whiteState = _playerStateService.CurrentPlayerState(clonedBoardState, Colours.White);
            var blackState = _playerStateService.CurrentPlayerState(clonedBoardState, Colours.Black);

            if (whiteState == PlayerState.InProgress && blackState == PlayerState.InProgress)
            {
                return GameCheckState.None;
            }

            if (whiteState != PlayerState.InProgress && blackState != PlayerState.InProgress)
            {
                throw new Exception($"Invalid game states white/black {whiteState}/{blackState}");
            }

            if (whiteState != PlayerState.InProgress)
            {
                return whiteState == PlayerState.Check
                    ? GameCheckState.WhiteInCheck
                    : GameCheckState.WhiteCheckmated;
            }

            if (blackState != PlayerState.InProgress)
            {
                return blackState == PlayerState.Check
                    ? GameCheckState.BlackInCheck
                    : GameCheckState.BlackCheckmated;
            }


            return GameCheckState.None;
        }

        public bool DoesMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move)
        {
            var checkColour = boardState.GetItem(move.From).Item.Player;
            
            var clonedBoardState = CreateCloneAndMove(boardState, move);

            var inCheck = _playerStateService.CurrentPlayerState(clonedBoardState, checkColour)
                          != PlayerState.InProgress;
            return inCheck;
        }


        public bool DoesMoveCauseCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move)
        {
            var checkColour = boardState.GetItem(move.From).Item.Player;
            
            var clonedBoardState = CreateCloneAndMove(boardState, move);

            var inCheck = _playerStateService.CurrentPlayerState(clonedBoardState, checkColour.Enemy()) !=
                          PlayerState.InProgress;
            return inCheck;
        }


        private IBoardState<ChessPieceEntity> CreateCloneAndMove(IBoardState<ChessPieceEntity> boardState,
            BoardMove move, Colours? refreshPathsColour = null)
        {
            var clonedBoardState = (IBoardState<ChessPieceEntity>) boardState.Clone();

            _moveService.Move(clonedBoardState, move);

            if (refreshPathsColour.HasValue)
            {
                clonedBoardState.RegeneratePaths((int) refreshPathsColour.Value);
            }
            else
            {
                clonedBoardState.RegenerateAllPaths();
            }

            return clonedBoardState;
        }
    }

    public interface ICheckDetectionService
    {
        bool DoesMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move);
        GameCheckState Check(IBoardState<ChessPieceEntity> boardState);
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