using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;
using Microsoft.Extensions.Logging;

namespace chess.engine.Movement
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
        private readonly ICheckDetectionService _checkDetectionService;

        public ChessRefreshAllPaths(
            ILogger<ChessRefreshAllPaths> logger,
            ICheckDetectionService checkDetectionService
            )
        {
            _logger = logger;
            _checkDetectionService = checkDetectionService;
        }
        public void RefreshAllPaths(IBoardState<ChessPieceEntity> boardState)
        {
            _logger?.LogDebug("Beginning ChessRefreshAllPaths process...");
            boardState.RegenerateAllPaths();

            var boardStateGetAllItemLocations = boardState.GetAllItemLocations.ToList();
            
            foreach (var loc in boardStateGetAllItemLocations)
            {
                RemovePathsThatContainMovesThatLeaveUsInCheck(boardState, loc);
            }

            _logger?.LogDebug($"ChessRefreshAllPaths process finished... {boardStateGetAllItemLocations.Count()} paths refreshed");
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
            foreach (var move in path)
            {
                var inCheck = _checkDetectionService.DoesMoveLeaveUsInCheck(boardState, move);

                if (!inCheck) validPath.Add(move);
            }

            return !validPath.Any() ? null : validPath;
        }

    }
}