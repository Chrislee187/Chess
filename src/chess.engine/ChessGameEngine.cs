using System.Collections.Generic;
using System.Linq;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    public class ChessGameEngine
    {
        public delegate ChessPieceEntity ChessPieceEntityProvider(BoardLocation location);

        private readonly Dictionary<BoardLocation, ChessPieceEntity> _entities = new Dictionary<BoardLocation, ChessPieceEntity>();
        private readonly Dictionary<BoardLocation, IEnumerable<Path>> _paths = new Dictionary<BoardLocation, IEnumerable<Path>>();

        private ChessMoveValidator _moveValidator;

        public ChessGameEngine()
        {
            _moveValidator = new ChessMoveValidator(SafeGetEntity);
        }


        public ChessGameEngine AddEntity(ChessPieceEntity create, BoardLocation startingLocation)
        {
            _entities.Add(startingLocation, create);
            _paths.Add(startingLocation, null);

            return this;
        }

        public ActiveBoardPiece PieceAt(BoardLocation location)
        {
            var boardPiece = SafeGetEntity(location);
            if (boardPiece == null) return null;

            var validPaths = ValidMovesForEntityAt(boardPiece, location);

            return new ActiveBoardPiece(boardPiece, validPaths);
        }

        public ActiveBoardPiece PieceAt(string location) => PieceAt((BoardLocation)location);

        private IEnumerable<Path> ValidMovesForEntityAt(ChessPieceEntity entityAt, BoardLocation boardLocation)
        {
            if (_paths.TryGetValue(boardLocation, out IEnumerable<Path> possiblePaths))
            {
                if (possiblePaths == null)
                {
                    possiblePaths = GeneratePossibleMoves(boardLocation, entityAt).ToList();

                    var validPaths = RemoveInvalidMoves(possiblePaths).ToList();
                    _paths[boardLocation] = validPaths;
                    return validPaths;
                }

                return possiblePaths;
            }

            return new List<Path>();
        }

        // MoveTypes are specific to the game of chess and therefore should NOT be in the BoardGameEnginge
        private IEnumerable<Path> RemoveInvalidMoves(IEnumerable<Path> possiblePaths)
        {
            var validPaths = new List<Path>();

            foreach (var possiblePath in possiblePaths)
            {
                var validPath = _moveValidator.ValidPath(possiblePath);

                if (validPath.Any())
                {
                    validPaths.Add(validPath);
                }
            }

            return validPaths;
        }

        private IEnumerable<Path> GeneratePossibleMoves(BoardLocation boardLocation, ChessPieceEntity entity)
        {
            var paths = new List<Path>();

            foreach (var pathGen in entity.PathGenerators)
            {
                var movesFrom = pathGen.PathsFrom(boardLocation, entity.Player);
                paths.AddRange(movesFrom);
            }

            return paths;
        }

        private ChessPieceEntity SafeGetEntity(BoardLocation location)
        {
            _entities.TryGetValue(location, out var entityAt);

            return entityAt;
        }
    }
}