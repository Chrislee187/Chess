using System;
using System.Linq;
using board.engine;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess.Entities;
using chess.engine.Game;
using Microsoft.Extensions.Logging;

namespace chess.engine.Chess
{
    /// <summary>
    /// Chess requires us to generate the paths in a certain way as for a move to be valid it must
    /// not leave the moving pieces king in check. To ascertain this we play out the move on a cloned
    /// copy of the board, regenerate all the moves now available the enemy player, and see if any of those
    /// attack the friendly king
    ///
    /// NB. This is hard to test in isolation (as it depends on so much state from the board to work) so there
    /// are no unit tests, any test that generates a board exercises this code however.
    /// </summary>
    public class ChessRefreshAllPaths : IRefreshAllPaths<ChessPieceEntity>
    {
        private readonly ILogger _logger;

        private readonly IBoardActionProvider<ChessPieceEntity> _actionProvider;
        // TODO: Sort of logger
        private readonly IChessGameStateService _chessGameStateService;

        public ChessRefreshAllPaths(
            ILogger<ChessRefreshAllPaths> logger,
            IBoardActionProvider<ChessPieceEntity> actionProvider,
                IChessGameStateService chessGameStateService
            )
        {
            _logger = logger;
            _actionProvider = actionProvider;
            _chessGameStateService = chessGameStateService;
        }
        public void RefreshAllPaths(IBoardState<ChessPieceEntity> boardState)
        {
            _logger.LogDebug("Beginning RefreshAllPaths process...");
            boardState.RegenerateAllPaths();

            var boardStateGetAllItemLocations = boardState.GetAllItemLocations.ToList();
            
            foreach (var loc in boardStateGetAllItemLocations)
            {
                RemovePathsThatContainMovesThatLeaveUsInCheck(boardState, loc);
            }

            _logger.LogDebug($"RefreshAllPaths process finished... {boardStateGetAllItemLocations.Count()} paths refreshed");
        }

        private void RemovePathsThatContainMovesThatLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardLocation loc)
        {
            var piece = boardState.GetItem(loc);
            var validPaths = new Paths();
            foreach (var path in piece.Paths)
            {
                var validPath = ValidatePathForDiscoveredCheck(boardState, path);
                if(validPath != null) validPaths.Add(validPath);
            }
            piece.UpdatePaths(validPaths);
        }

        private Path ValidatePathForDiscoveredCheck(IBoardState<ChessPieceEntity> boardState, Path path)
        {
            var validPath = new Path();
            var pieceColour = boardState.GetItem(path.First().From).Item.Player;
            foreach (var move in path)
            {
                var inCheck = DoeMoveLeaveUsInCheck(boardState, move, pieceColour);

                if (!inCheck) validPath.Add(move);
            }

            return !validPath.Any() ? null : validPath;
        }

        private bool DoeMoveLeaveUsInCheck(ICloneable boardState, BoardMove move, Colours pieceColour)
        {
            var clonedBoardState = (IBoardState<ChessPieceEntity>) boardState.Clone();
            var action = _actionProvider.Create(move.MoveType, clonedBoardState);
            action.Execute(move);

            clonedBoardState.RegeneratePaths((int)pieceColour.Enemy());

            var inCheck = _chessGameStateService.CurrentGameState(clonedBoardState, pieceColour)
                          != GameState.InProgress;
            return inCheck;
        }
    }
}