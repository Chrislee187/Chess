using System.Collections.Generic;
using System.Collections.ObjectModel;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    public class BoardState
    {

        private readonly ChessMoveValidator _moveValidator;
        private readonly IDictionary<BoardLocation, ChessPieceEntity> _entities;
        private readonly IDictionary<BoardLocation, IEnumerable<Path>> _paths;

        public IReadOnlyDictionary<BoardLocation, ChessPieceEntity> Entities => new ReadOnlyDictionary<BoardLocation, ChessPieceEntity>(_entities);
        public IReadOnlyDictionary<BoardLocation, IEnumerable<Path>> Paths => new ReadOnlyDictionary<BoardLocation, IEnumerable<Path>>(_paths);

        public BoardState(
            IDictionary<BoardLocation, ChessPieceEntity> entities,
            IDictionary<BoardLocation, IEnumerable<Path>> paths
            )
        {
            _moveValidator = new ChessMoveValidator();
            _entities = entities;
            _paths = paths;
        }


        public Path ValidPath(Path possiblePath, BoardState boardState) 
            => _moveValidator.ValidPath(possiblePath, boardState);

        public void SetPaths(BoardLocation loc, IEnumerable<Path> paths) 
            => _paths[loc] = paths;

        public void SetEntity(BoardLocation loc, ChessPieceEntity entity) 
            => _entities[loc] = entity;

        public IEnumerable<Path> GetPathsOrNull(BoardLocation loc) 
            => Paths.TryGetValue(loc, out var paths) 
                ? paths 
                : null;

        public ChessPieceEntity GetEntityOrNull(BoardLocation loc)
            => Entities.TryGetValue(loc, out var entity)
                ? entity
                : null;
    }
}