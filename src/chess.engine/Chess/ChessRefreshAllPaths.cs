using System.Diagnostics;
using System.Linq;
using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess
{
    /// <summary>
    /// Chess requires us to generate the paths in a certain way as for a move to be valid it must
    /// not leave the moving pieces king in check. To ascertain this we play out the move on a cloned
    /// copy of the board, regenerate all the moves now available the enemy player, and see if any of those
    /// attack the friendly king
    /// </summary>
    public class ChessRefreshAllPaths : IRefreshAllPaths<ChessPieceEntity>
    {
        /*
         * Generate 1st level paths (no check detection)
         * foreach move
         *      play move on cloned board
         *      generate paths with check detection for OPPOSITE side only
         *      check for in-check
         *
         */
         private readonly IBoardActionFactory<ChessPieceEntity> _actionFactory = new BoardActionFactory<ChessPieceEntity>();
         private readonly IChessGameState _chessGameState = new ChessGameState();
        public void RefreshAllPaths(IBoardState<ChessPieceEntity> boardState)
        {
            boardState.RegenerateAllPaths();

            foreach (var loc in boardState.GetAllItemLocations)
            {
//                if(loc.X == 5 && loc.Y == 8) Debugger.Break();
                RemovePathsThatContainMovesThatLeaveUsInCheck(boardState, loc);
            }
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
            var pieceColour = boardState.GetItem(path.First().From).Item.Owner;
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

            clonedBoardState.RegeneratePaths(pieceColour.Enemy());

            var inCheck = _chessGameState.CurrentGameState(clonedBoardState, pieceColour)
                          != GameState.InProgress;
            return inCheck;
        }
    }
}