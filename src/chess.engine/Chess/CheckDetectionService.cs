using System;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using Microsoft.Extensions.Logging;

namespace chess.engine.Chess
{
    public class CheckDetectionService : ICheckDetectionService
    {
        private readonly ILogger<CheckDetectionService> _logger;
        private readonly IBoardActionProvider<ChessPieceEntity> _actionProvider;
        private readonly IChessGameStateService _chessGameStateService;

        public CheckDetectionService(
            ILogger<CheckDetectionService> logger,
            IBoardActionProvider<ChessPieceEntity> actionProvider, 
            IChessGameStateService chessGameStateService
        )
        {
            _logger = logger;
            _actionProvider = actionProvider;
            _chessGameStateService = chessGameStateService;
        }

        public GameCheckState Check(IBoardState<ChessPieceEntity> boardState)
        {
            var clonedBoardState = (IBoardState<ChessPieceEntity>)boardState.Clone();

            var whiteState = _chessGameStateService.CurrentGameState(clonedBoardState, Colours.White);
            var blackState = _chessGameStateService.CurrentGameState(clonedBoardState, Colours.Black);

            if (whiteState == GameState.InProgress && blackState == GameState.InProgress)
            {
                return GameCheckState.None;
            }

            if (whiteState != GameState.InProgress && blackState != GameState.InProgress)
            {
                throw new Exception($"Invalid game states white/black {whiteState}/{blackState}");
            }

            if (whiteState != GameState.InProgress)
            {
                return whiteState == GameState.Check
                    ? GameCheckState.WhiteInCheck
                    : GameCheckState.WhiteCheckmated;
            }

            if (blackState != GameState.InProgress)
            {
                return blackState == GameState.Check
                    ? GameCheckState.BlackInCheck
                    : GameCheckState.BlackCheckmated;
            }


            return GameCheckState.None; 
        }

        public bool DoeMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move, Colours pieceColour)
        {
            var clonedBoardState = CreateClone(boardState, move, pieceColour);

            var inCheck = _chessGameStateService.CurrentGameState(clonedBoardState, pieceColour)
                          != GameState.InProgress;
            return inCheck;
        }

        private IBoardState<ChessPieceEntity> CreateClone(IBoardState<ChessPieceEntity> boardState, BoardMove move, Colours pieceColour)
        {
            var clonedBoardState = (IBoardState<ChessPieceEntity>) boardState.Clone();
            var action = _actionProvider.Create(move.MoveType, clonedBoardState);
            action.Execute(move);

            clonedBoardState.RegeneratePaths((int) pieceColour.Enemy());
            return clonedBoardState;
        }
    }

    public interface ICheckDetectionService
    {
        bool DoeMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move, Colours pieceColour);
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