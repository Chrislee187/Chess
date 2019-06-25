using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Extensions;
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

        public Paths GetValidatedPaths(IBoardState<ChessPieceEntity> boardState, ChessPieceEntity entity, BoardLocation boardLocation)
        {
            var paths = new Paths(
                entity.PathGenerators
                    .SelectMany(pg => pg.PathsFrom(boardLocation, entity.Owner))
            );

            var validPaths = RemoveInvalidMoves2(boardState, paths);

            return validPaths;
        }

        private Paths RemoveInvalidMoves(IBoardState<ChessPieceEntity> boardState, Paths possiblePaths)
        {
            var validPaths = new Paths();

            possiblePaths.ForEach(possiblePath =>
            {
                ValidatePath(boardState, possiblePath, validPaths);
            });

            return validPaths;
        }

        private void ValidatePath(IBoardState<ChessPieceEntity> boardState, Path possiblePath, Paths validPaths)
        {
            var testedPath = _pathValidator.ValidatePath(boardState, possiblePath);

            if (testedPath.Any())
            {
                validPaths.Add(testedPath);
            }
        }

        private Paths RemoveInvalidMoves2(IBoardState<ChessPieceEntity> boardState, Paths possiblePaths) =>
            new Paths(
                possiblePaths
                    .Where(possiblePath => _pathValidator.ValidatePath(boardState, possiblePath).Any())
                    .Select(pp => _pathValidator.ValidatePath(boardState, pp))
            );
    }
}