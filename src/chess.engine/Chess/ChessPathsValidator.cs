using System.Linq;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Chess
{
    public class ChessPathsValidator : IPathsValidator<ChessPieceEntity>
    {
        private readonly IPathValidator<ChessPieceEntity> _pathValidator;

        public ChessPathsValidator(IPathValidator<ChessPieceEntity> pathValidator)
        {
            _pathValidator = pathValidator;
        }

        public Paths GeneratePossiblePaths(IBoardState<ChessPieceEntity> boardState, ChessPieceEntity entity, BoardLocation boardLocation)
        {
            var paths = new Paths();
            paths.AddRange(
                entity.PathGenerators
                    .SelectMany(pg => pg.PathsFrom(boardLocation, entity.Owner))
            );

            return RemoveInvalidMoves(boardState, paths);
        }

        private Paths RemoveInvalidMoves(IBoardState<ChessPieceEntity> boardState, Paths possiblePaths)
        {
            var validPaths = new Paths();

            foreach (var possiblePath in possiblePaths)
            {
                var testedPath = _pathValidator.ValidatePath(boardState, possiblePath);

                if (testedPath.Any())
                {
                    validPaths.Add(testedPath);
                }
            }

            return validPaths;
        }

    }
}