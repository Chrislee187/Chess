using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    public class ChessBoardEngine : IBoardActions
    {
        public delegate ChessPieceEntity ChessPieceEntityProvider(BoardLocation location);

        internal readonly Dictionary<BoardLocation, ChessPieceEntity> _entities = new Dictionary<BoardLocation, ChessPieceEntity>();
        internal readonly Dictionary<BoardLocation, IEnumerable<Path>> _paths = new Dictionary<BoardLocation, IEnumerable<Path>>();

        private readonly ChessMoveValidator _moveValidator;

        public ChessBoardEngine()
        {
            _moveValidator = new ChessMoveValidator(SafeGetEntity);
        }

        public ChessBoardEngine InitBoard()
        {
            //            foreach (var rank in new []{1,8})
            //            {
            //                var colour = rank == 1 ? Colours.White : Colours.Black;
            //
            //                AddEntity(ChessPieceEntityFactory.CreateRook(colour), BoardLocation.At($"A{rank}"));
            //                AddEntity(ChessPieceEntityFactory.CreateKnight(colour), BoardLocation.At($"B{rank}"));
            //                AddEntity(ChessPieceEntityFactory.CreateBishop(colour), BoardLocation.At($"C{rank}"));
            //                AddEntity(ChessPieceEntityFactory.CreateQueen(colour),  BoardLocation.At($"D{rank}"));
            //                AddEntity(ChessPieceEntityFactory.CreateKing(colour),   BoardLocation.At($"E{rank}"));
            //                AddEntity(ChessPieceEntityFactory.CreateBishop(colour), BoardLocation.At($"F{rank}"));
            //                AddEntity(ChessPieceEntityFactory.CreateKnight(colour), BoardLocation.At($"G{rank}"));
            //                AddEntity(ChessPieceEntityFactory.CreateRook(colour),   BoardLocation.At($"H{rank}"));
            //
            //            }

            foreach (var colour in new[] {Colours.White, Colours.Black})
            {
                foreach (var file in Enum.GetValues(typeof(ChessFile)))
                {
                    AddEntity(ChessPieceEntityFactory.CreatePawn(colour),
                        BoardLocation.At((ChessFile) file, colour == Colours.White ? 2 : 7));
                }
            }

            return this;
        }

        public ChessBoardEngine AddEntity(ChessPieceEntity create, string startingLocation) =>
            AddEntity(create, BoardLocation.At(startingLocation));
        public ChessBoardEngine AddEntity(ChessPieceEntity create, BoardLocation startingLocation)
        {
            _entities.Add(startingLocation, create);
            _paths.Add(startingLocation, null);

            return this;
        }


        public ActiveBoardPiece PieceAt(string location) => PieceAt((BoardLocation)location);
        public ActiveBoardPiece PieceAt(BoardLocation location)
        {
            var boardPiece = SafeGetEntity(location);
            if (boardPiece == null) return null;

            var validPaths = ValidMovesForEntityAt(boardPiece, location);

            return new ActiveBoardPiece(boardPiece, validPaths);
        }

        public void Move(ChessMove validMove)
        {
            Action<ChessMove, IBoardActions> action;
            switch (validMove.ChessMoveType)
            {
                case ChessMoveType.MoveOnly:
                    action = MoveOnlyAction;
                    break;
                default:
                    throw new NotImplementedException($"MoveType: {validMove.ChessMoveType} not implemented");
            }

            action(validMove, this);
        }

        public BoardPiece[,] Board
        {
            get
            {
                var pieces = new BoardPiece[8, 8];
                foreach (ChessFile file in Enum.GetValues(typeof(ChessFile)))
                {
                    for (int rank = 8; rank > 1; rank--)
                    {
                        var entity = SafeGetEntity(BoardLocation.At(file, rank));
                        pieces[(int)file - 1, rank - 1] = entity == null
                            ? null
                            : new BoardPiece(entity.Player, entity.EntityType);
                    }
                }

                return pieces;
            }
        }

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

        private IEnumerable<Path> ValidMovesForEntityAt(ChessPieceEntity entityAt, BoardLocation boardLocation)
        {
            if (_paths.TryGetValue(boardLocation, out IEnumerable<Path> paths))
            {
                if (paths == null)
                {
                    paths = GeneratePossibleMoves(boardLocation, entityAt).ToList();

                    paths = RemoveInvalidMoves(paths).ToList();
                    _paths[boardLocation] = paths;
                }

                return paths;
            }

            return new List<Path>();
        }

        private ChessPieceEntity SafeGetEntity(BoardLocation location)
        {
            _entities.TryGetValue(location, out var entityAt);

            return entityAt;
        }

        #region Board Actions
        void MoveOnlyAction(ChessMove move, IBoardActions actions)
        {
            var piece = actions.GetEntity(move.From);
            actions.ClearSquare(move.From);
            actions.ClearSquare(move.To);
            actions.SetEntity(move.To, piece);
        }

        ChessPieceEntity IBoardActions.GetEntity(BoardLocation loc) => SafeGetEntity(loc);

        void IBoardActions.SetEntity(BoardLocation loc, ChessPieceEntity entity)
        {
            _entities[loc] = entity;
        }

        void IBoardActions.ClearSquare(BoardLocation loc)
        {
            _entities[loc] = null;
            _paths[loc] = null;

        }

        #endregion

    }
}