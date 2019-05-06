using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess
{
    public class ChessRefreshAllPaths : IRefreshAllPaths<ChessPieceEntity>
    {
        /*
         * create normal paths (without discovered check validation) for all pieces
         * -- king check
         * foreach move
         *      play move on cloned board
         *      generate normal paths for OPPOSITE side only
         *      see if king is in check
         *
         */
         private readonly IBoardActionFactory<ChessPieceEntity> _actionFactory = new BoardActionFactory<ChessPieceEntity>();

        public void RefreshAllPaths(IBoardState<ChessPieceEntity> boardState)
        {
            boardState.RegenerateAllPaths();

            foreach (var loc in boardState.GetAllItemLocations)
            {
                ValidatePathsForDiscoveredCheck(boardState, loc);
            }
        }

        private void ValidatePathsForDiscoveredCheck(IBoardState<ChessPieceEntity> boardState, BoardLocation loc)
        {
            var piece = boardState.GetItem(loc);
            var pieceColour = piece.Item.Owner;
            var enemyColour = pieceColour.Enemy();
            var validPaths = new Paths();
            foreach (var path in piece.Paths)
            {
                ValidatePathForDiscoveredCheck(boardState, path, enemyColour, pieceColour, validPaths);
            }

            piece.UpdatePaths(validPaths);
        }

        private void ValidatePathForDiscoveredCheck(IBoardState<ChessPieceEntity> boardState, Path path, Colours enemyColour, Colours pieceColour, Paths validPaths)
        {
            var validPath = new Path();
            foreach (var move in path)
            {
                var clone = boardState.Clone() as IBoardState<ChessPieceEntity>;
                var action = _actionFactory.Create(move.MoveType, clone);
                action.Execute(move);

                clone.RegeneratePaths(enemyColour);

                var inCheck = clone.CurrentGameState(pieceColour, enemyColour)
                              != GameState.InProgress;

                if (!inCheck) validPath.Add(move);
            }

            validPaths.Add(validPath);
        }

    }
}