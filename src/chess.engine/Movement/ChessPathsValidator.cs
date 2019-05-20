using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using Microsoft.Extensions.Logging;

namespace chess.engine.Movement
{
    public class ChessPathsValidator : IPathsValidator<ChessPieceEntity>
    {
        private readonly IPathValidator<ChessPieceEntity> _pathValidator;
        private readonly ILogger<ChessPathsValidator> _logger;

        public ChessPathsValidator(
            ILogger<ChessPathsValidator> logger,
            IPathValidator<ChessPieceEntity> pathValidator
            )
        {
            _logger = logger;
            _pathValidator = pathValidator;
        }

        public Paths GeneratePossiblePaths(IBoardState<ChessPieceEntity> boardState, ChessPieceEntity entity, BoardLocation boardLocation)
        {
            _logger?.LogDebug($"Generating possible paths for {entity} at {boardLocation}.");

            var paths = new Paths();
            paths.AddRange(
                entity.PathGenerators
                    .SelectMany(pg => pg.PathsFrom(boardLocation, (int) entity.Player))
            );

            var validPaths = FeatureFlags.ParalleliseRemoveInvalidMoves 
                ? RemoveInvalidMovesParallel(boardState, paths) 
                : RemoveInvalidMoves(boardState, paths);

            _logger?.LogDebug($"Valid paths for {entity} at {boardLocation}. {validPaths}");

            return validPaths;
        }

        private Paths RemoveInvalidMoves(IBoardState<ChessPieceEntity> boardState, Paths possiblePaths)
        {
            _logger?.LogDebug($"Removing invalid moves from {possiblePaths} paths.");
            var validPaths = new Paths();

            possiblePaths.ToList().ForEach(possiblePath =>
            {
                var testedPath = _pathValidator.ValidatePath(boardState, possiblePath);

                if (testedPath.Any())
                {
                    validPaths.Add(testedPath);
                }
                else
                {
                    _logger?.LogDebug($"Removed {possiblePath}.");
                }
            });

            return validPaths;
        }


        private Paths RemoveInvalidMovesParallel(IBoardState<ChessPieceEntity> boardState, Paths possiblePaths)
        {
            _logger?.LogDebug($"Removing invalid moves from {possiblePaths} paths.");
            var validPaths = new Paths();

            possiblePaths.AsParallel().ForAll(possiblePath =>
            {
                var testedPath = _pathValidator.ValidatePath(boardState, possiblePath);

                if (testedPath.Any())
                {
                    validPaths.Add(testedPath);
                }
                else
                {
                    _logger?.LogDebug($"Removed {possiblePath}.");
                }
            });

            return validPaths;
        }
    }
}