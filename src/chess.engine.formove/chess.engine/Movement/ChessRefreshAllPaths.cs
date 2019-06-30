using System.Collections.Generic;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;
using Microsoft.Extensions.Logging;

namespace chess.engine.Movement
{
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


        private readonly IDictionary<string, LocatedItem<ChessPieceEntity>[]> _stateCache
        = new Dictionary<string, LocatedItem<ChessPieceEntity>[]>();

        public void RefreshAllPaths(IBoardState<ChessPieceEntity> boardState)
        {
            _logger?.LogDebug("Beginning ChessRefreshAllPaths process...");

            RefreshPathsFeature(boardState);

            // NOTE: IMPORTANT: Kings must be evaluated last to ensure that moves
            // from other pieces that would cause check are generated first!
            // kings have an EntityType of int.MaxValue
            var boardStateGetAllItemLocations = boardState.GetItems()
                .OrderBy(i => i.Item.EntityType)
                .Select(i => i.Location).ToList();
            
            foreach (var loc in boardStateGetAllItemLocations)
            {
                RemovePathsThatContainMovesThatLeaveUsInCheck(boardState, loc);
            }

            _logger?.LogDebug($"ChessRefreshAllPaths process finished... {boardStateGetAllItemLocations.Count()} paths refreshed");
        }

        private void RefreshPathsFeature(IBoardState<ChessPieceEntity> boardState)
        {
            if (FeatureFlags.CachingPaths)
            {
                // Need proper boardstate key I think, currently a few tests fail, I guess around some state related
                // so something not encoded in the textboard (enpassant  and castle viability namely)
                var stateKey = ChessGameConvert.SerialiseBoard(boardState);
                if (_stateCache.TryGetValue(stateKey, out var items))
                {
                    boardState.UpdatePaths(items);
                }
                else
                {

                    RefreshChessPaths(boardState);

                    if (FeatureFlags.CachingPaths)
                    {
                        _stateCache.Add(stateKey, boardState.GetItems().ToArray());
                    }
                }
            }
            else
            {
                RefreshChessPaths(boardState);
            }
        }

        private static void RefreshChessPaths(IBoardState<ChessPieceEntity> boardState)
        {
            // NOTE: care must be taken refreshing all the paths on the chess board
            // as kings need to know the enemy piece paths before their moves can be validated
            // (so as not to move in to check etc.)
            // 
            // As the boardState refresh parallelises the calls we must do the kings after all the others
            // to avoid race conditions with a king trying to validate before all the enemy pieces have complete
            // move lists (took nearly 5500 games being processed to finally track this one down!)

            var nonKings = boardState.GetItems().Where(i => i.Item.EntityType != (int) ChessPieceName.King);
            var kings = boardState.GetItems().Where(i => i.Item.EntityType == (int) ChessPieceName.King);

            boardState.RefreshPathsFor(nonKings);
            boardState.RefreshPathsFor(kings);

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
            var validPath = new Path(
                    path.Where(move => !_checkDetectionService.DoesMoveLeaveUsInCheck(boardState, move))
                );
            
            return !validPath.Any() ? null : validPath;
        }
    }
}