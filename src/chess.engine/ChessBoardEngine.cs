﻿using System;
using System.Collections.Generic;
using System.Linq;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    public class ChessBoardEngine : IBoardActions
    {
        private readonly BoardState _boardState = new BoardState(new Dictionary<BoardLocation, ChessPieceEntity>(),new Dictionary<BoardLocation, IEnumerable<Path>>());
        
        public ChessBoardEngine InitBoard()
        {
            foreach (var rank in new []{1,8})
            {
                var colour = rank == 1 ? Colours.White : Colours.Black;

                AddEntity(ChessPieceEntityFactory.CreateRook(colour), BoardLocation.At($"A{rank}"));
                AddEntity(ChessPieceEntityFactory.CreateKnight(colour), BoardLocation.At($"B{rank}"));
                AddEntity(ChessPieceEntityFactory.CreateBishop(colour), BoardLocation.At($"C{rank}"));
//                PlaceEntity(ChessPieceEntityFactory.CreateQueen(colour),  BoardLocation.At($"D{rank}"));
                AddEntity(ChessPieceEntityFactory.CreateKing(colour),   BoardLocation.At($"E{rank}"));
                AddEntity(ChessPieceEntityFactory.CreateBishop(colour), BoardLocation.At($"F{rank}"));
                AddEntity(ChessPieceEntityFactory.CreateKnight(colour), BoardLocation.At($"G{rank}"));
                AddEntity(ChessPieceEntityFactory.CreateRook(colour),   BoardLocation.At($"H{rank}"));
            
            }

            foreach (var colour in new[] {Colours.White, Colours.Black})
            {
                foreach (var file in Enum.GetValues(typeof(ChessFile)))
                {
                    AddEntity(ChessPieceEntityFactory.CreatePawn(colour),
                        BoardLocation.At((ChessFile) file, colour == Colours.White ? 2 : 7));
                }
            }

            // Calculate the valid moves for each piece
            // Do kings last and seperate to avoid recursion when they check whether possible moves are underattack from enemy pieces
            RecalculatePaths();

            return this;
        }

        private void RecalculatePaths()
        {
            foreach (var kvp in _boardState.Entities)
            {
                var piece = kvp.Value;
                _boardState.SetPaths(kvp.Key, null);
                if (piece.EntityType != ChessPieceName.King)
                {
                    ValidMovesForEntityAt(kvp.Value, kvp.Key);
                }
            }

            var kings = _boardState.Entities.Where(kvp => kvp.Value.EntityType == ChessPieceName.King).ToList();

            ValidMovesForEntityAt(kings[0].Value, kings[0].Key);
            ValidMovesForEntityAt(kings[1].Value, kings[1].Key);
        }

        public ChessBoardEngine AddEntity(ChessPieceEntity create, string startingLocation) =>
            AddEntity(create, BoardLocation.At(startingLocation));
        public ChessBoardEngine AddEntity(ChessPieceEntity create, BoardLocation startingLocation)
        {
            _boardState.SetEntity(startingLocation, create);
            _boardState.SetPaths(startingLocation, null);
            return this;
        }
        
        public ActiveBoardPiece PieceAt(string location) => PieceAt((BoardLocation)location);
        public ActiveBoardPiece PieceAt(BoardLocation location)
        {
            var piece = SafeGetEntity(location);
            if (piece == null) return null;

            var validPaths = ValidMovesForEntityAt(piece, location);

            return new ActiveBoardPiece(piece, validPaths);
        }

        public void Move(ChessMove validMove)
        {
            Action<ChessMove, IBoardActions> action;
            switch (validMove.ChessMoveType)
            {
                case ChessMoveType.MoveOnly:
                    action = MoveOnlyAction;
                    break;
                case ChessMoveType.TakeOnly:
                    action = TakeOnlyAction;
                    break;
                case ChessMoveType.KingMove:
                    action = MoveOnlyAction;
                    break;
                case ChessMoveType.MoveOrTake:
                    action = MoveOrTakeAction;
                    break;
                default:
                    throw new NotImplementedException($"MoveType: {validMove.ChessMoveType} not implemented");
            }

            action(validMove, this);
            RecalculatePaths();
        }

        public BoardPiece[,] Board
        {
            get
            {
                var pieces = new BoardPiece[8, 8];
                foreach (ChessFile file in Enum.GetValues(typeof(ChessFile)))
                {
                    for (int rank = 8; rank > 0; rank--)
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
                var validPath = _boardState.ValidPath(possiblePath, _boardState);

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
            var paths = _boardState.GetPathsOrNull(boardLocation);

            if (paths == null)
            {
                paths = GeneratePossibleMoves(boardLocation, entityAt).ToList();

                paths = RemoveInvalidMoves(paths).ToList();
                _boardState.SetPaths(boardLocation, paths);
            }

            return paths;
        }

        private ChessPieceEntity SafeGetEntity(BoardLocation location)
        {
            _boardState.Entities.TryGetValue(location, out var entityAt);

            return entityAt;
        }

        #region Board Actions
        void MoveOnlyAction(ChessMove move, IBoardActions actions)
        {
            var piece = actions.GetEntity(move.From);
            actions.ClearSquare(move.From);
            actions.PlaceEntity(move.To, piece);
        }
        void TakeOnlyAction(ChessMove move, IBoardActions actions)
        {
            var piece = actions.GetEntity(move.From);

            TakePiece(move.To, actions);

            MoveOnlyAction(move, actions);
        }

        void MoveOrTakeAction(ChessMove move, IBoardActions actions)
        {
            var dest = actions.GetEntity(move.To);
            
            if (dest != null)
            {
                TakePiece(move.To, actions);
            }

            MoveOnlyAction(move, actions);
        }

        void TakePiece(BoardLocation loc, IBoardActions actions)
        {

            // TODO: Record lost piece etc.
            actions.ClearSquare(loc);
        }

        ChessPieceEntity IBoardActions.GetEntity(BoardLocation loc) => SafeGetEntity(loc);

        void IBoardActions.PlaceEntity(BoardLocation loc, ChessPieceEntity entity)
        {
            _boardState.SetEntity(loc, entity);
            _boardState.SetPaths(loc, GeneratePossibleMoves(loc, entity));
        }

        void IBoardActions.ClearSquare(BoardLocation loc)
        {
            _boardState.SetEntity(loc, null);
            _boardState.SetPaths(loc, null);
        }
        #endregion

    }
}