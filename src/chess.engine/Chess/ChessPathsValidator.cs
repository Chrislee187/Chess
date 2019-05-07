using System.Linq;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;
using Microsoft.Extensions.Logging;

namespace chess.engine.Chess
{
    public class ChessPathsValidator : IPathsValidator<ChessPieceEntity>
    {
        private readonly IPathValidator<ChessPieceEntity> _pathValidator;
        private readonly ILogger<ChessPathValidator> _logger;

        public ChessPathsValidator(
            ILogger<ChessPathValidator> logger,
            IPathValidator<ChessPieceEntity> pathValidator
            )
        {
            _logger = logger;
            _pathValidator = pathValidator;
        }

        public Paths GeneratePossiblePaths(IBoardState<ChessPieceEntity> boardState, ChessPieceEntity entity, BoardLocation boardLocation)
        {
            _logger.LogDebug($"Generating possible paths for {entity} at {boardLocation}.");
            var paths = new Paths();
            paths.AddRange(
                entity.PathGenerators
                    .SelectMany(pg => pg.PathsFrom(boardLocation, entity.Player))
            );

            var validPaths = RemoveInvalidMoves(boardState, paths);
            _logger.LogDebug($"Valid paths for {entity} at {boardLocation}. {validPaths}");

            return validPaths;
        }

        private Paths RemoveInvalidMoves(IBoardState<ChessPieceEntity> boardState, Paths possiblePaths)
        {
            _logger.LogDebug($"Removing invalid moves from {possiblePaths} paths.");
            var validPaths = new Paths();

            foreach (var possiblePath in possiblePaths)
            {
                var testedPath = _pathValidator.ValidatePath(boardState, possiblePath);

                if (testedPath.Any())
                {
                    validPaths.Add(testedPath);
                }
                else
                {
                    _logger.LogDebug($"Removed {possiblePath}.");
                }
            }

            return validPaths;
        }
    }
}