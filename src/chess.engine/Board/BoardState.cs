using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    public class BoardState
    {
        private readonly IPathValidator _pathValidator;
        private readonly IDictionary<BoardLocation, ChessPieceEntity> _entities;
        private readonly IDictionary<BoardLocation, IEnumerable<Path>> _paths;

        public IReadOnlyDictionary<BoardLocation, ChessPieceEntity> Entities => new ReadOnlyDictionary<BoardLocation, ChessPieceEntity>(_entities);
        public IReadOnlyDictionary<BoardLocation, IEnumerable<Path>> Paths => new ReadOnlyDictionary<BoardLocation, IEnumerable<Path>>(_paths);

        public BoardState(IPathValidator pathValidator)
        {
            _pathValidator = pathValidator;
            _entities = new Dictionary<BoardLocation, ChessPieceEntity>();
            _paths = new Dictionary<BoardLocation, IEnumerable<Path>>();
        }

        public Path ValidPath(Path possiblePath) => _pathValidator.ValidatePath(possiblePath, this);

        public void SetPaths(BoardLocation loc, IEnumerable<Path> paths) => _paths[loc] = paths;

        public void ClearPaths(BoardLocation loc) => _paths.Remove(loc);

        public void SetEntity(BoardLocation loc, ChessPieceEntity entity)
        {
            if (entity == null)
            {
                _entities.Remove(loc);
            }
            else
            {
                _entities[loc] = entity;
            }
        }

        public IEnumerable<Path> GetPathsOrNull(BoardLocation loc) 
            => Paths.TryGetValue(loc, out var paths) 
                ? paths 
                : null;

        public ChessPieceEntity GetEntityOrNull(BoardLocation loc)
            => Entities.TryGetValue(loc, out var entity)
                ? entity
                : null;

        public IEnumerable<Path> GetOrCreatePaths(ChessPieceEntity entityAt, BoardLocation boardLocation)
        {
            var paths = GetPathsOrNull(boardLocation);

            if (paths == null)
            {
                paths = GeneratePossiblePaths(entityAt, boardLocation).ToList();

                paths = RemoveInvalidMoves(paths).ToList();
                SetPaths(boardLocation, paths);
            }

            return paths;
        }

        public IEnumerable<Path> GeneratePossiblePaths(ChessPieceEntity entity, BoardLocation boardLocation)
        {
            var paths = new List<Path>();

            foreach (var pathGen in entity.PathGenerators)
            {
                var movesFrom = pathGen.PathsFrom(boardLocation, entity.Player);
                paths.AddRange(movesFrom);
            }

            return paths;
        }

        private IEnumerable<Path> RemoveInvalidMoves(IEnumerable<Path> possiblePaths)
        {
            var validPaths = new List<Path>();

            foreach (var possiblePath in possiblePaths)
            {
                var validPath = ValidPath(possiblePath);

                if (validPath.Any())
                {
                    validPaths.Add(validPath);
                }
            }

            return validPaths;
        }

        public void Clear()
        {
            _entities.Clear();
            _paths.Clear();
        }
    }
}