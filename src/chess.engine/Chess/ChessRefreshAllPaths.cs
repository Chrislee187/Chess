using System.Linq;
using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

        private readonly IBoardActionFactory<ChessPieceEntity> _actionFactory = new BoardActionFactory<ChessPieceEntity>();
        // TODO: Sort of logger
        private readonly IChessGameState _chessGameState;

        public ChessRefreshAllPaths(
            ILogger<ChessRefreshAllPaths> logger,
                IChessGameState chessGameState
            )
        {
            _logger = logger;
            _chessGameState = chessGameState;
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

        private bool DoeMoveLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardMove move, Colours pieceColour)
        {
            var clonedBoardState = (IBoardState<ChessPieceEntity>) boardState.Clone();
            var action = _actionFactory.Create(move.MoveType, clonedBoardState);
            action.Execute(move);

            clonedBoardState.RegeneratePaths((int)pieceColour.Enemy());

            var inCheck = _chessGameState.CurrentGameState(clonedBoardState, pieceColour)
                          != GameState.InProgress;
            return inCheck;
        }
    }
}