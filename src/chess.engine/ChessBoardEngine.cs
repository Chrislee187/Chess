using System;
using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{

    public class ChessBoardEngine : IBoardState
    {
        public readonly BoardState BoardState;
        private readonly BoardActionFactory _boardActionFactory;

        private readonly IGameSetup _gameSetup;
        private readonly IRefreshAllPaths _allPathCalculator;

        public ChessBoardEngine(IGameSetup gameSetup, IPathValidator pathValidator) : this(gameSetup, pathValidator, new DefaultRefreshAllPaths())
        {
        }

        public ChessBoardEngine(IGameSetup gameSetup, IPathValidator pathValidator, IRefreshAllPaths allPathCalculator)
        {
            _boardActionFactory = new BoardActionFactory();

            BoardState = new BoardState(pathValidator);

            _gameSetup = gameSetup;
            _gameSetup.SetupPieces(this);

            _allPathCalculator = allPathCalculator;
            _allPathCalculator.RefreshAllPaths(BoardState);
        }

        public void ResetBoard()
        {
            ClearBoard();
            _gameSetup.SetupPieces(this);
        }
        public void ClearBoard()
        {
            BoardState.Clear();
        }

        public ChessBoardEngine AddEntity(ChessPieceEntity create, string startingLocation) =>
            AddEntity(create, BoardLocation.At(startingLocation));
        public ChessBoardEngine AddEntity(ChessPieceEntity create, BoardLocation startingLocation)
        {
            BoardState.SetEntity(startingLocation, create);
            BoardState.ClearPaths(startingLocation);
            return this;
        }
        
        public ActiveBoardPiece PieceAt(string location) => PieceAt((BoardLocation)location);
        public ActiveBoardPiece PieceAt(BoardLocation location)
        {
            var piece = BoardState.GetEntityOrNull(location);
            if (piece == null) return null;

            var validPaths = BoardState.GetOrCreatePaths(piece, location);

            return new ActiveBoardPiece(piece, validPaths);
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
                        var entity = BoardState.GetEntityOrNull(BoardLocation.At(file, rank));
                        pieces[(int)file - 1, rank - 1] = entity == null
                            ? null
                            : new BoardPiece(entity.Player, entity.EntityType);
                    }
                }

                return pieces;
            }
        }

        //TODO: Need an abstraction around MoveType's and Actions, to not be Chess specific
        // so will need some default state (move entity, remove entity) but can be extended with custom ones, (enpassant, castle)
        public void Move(ChessMove validMove)
        {
            var action = _boardActionFactory.Create(validMove.ChessMoveType, this);

            action.Execute(validMove);

            _allPathCalculator.RefreshAllPaths(BoardState);
        }


        #region Board Actions
        ChessPieceEntity IBoardState.GetEntity(BoardLocation loc) => BoardState.GetEntityOrNull(loc);

        void IBoardState.SetEntity(BoardLocation loc, ChessPieceEntity entity)
        {
            BoardState.SetEntity(loc, entity);
            BoardState.SetPaths(loc, BoardState.GeneratePossiblePaths(entity, loc));
        }

        void IBoardState.ClearLocation(BoardLocation loc)
        {
            BoardState.SetEntity(loc, null);
            BoardState.SetPaths(loc, null);
        }
        #endregion

        private class DefaultRefreshAllPaths : IRefreshAllPaths
        {
            public void RefreshAllPaths(BoardState boardState)
            {
                foreach (var kvp in boardState.Entities)
                {
                    boardState.SetPaths(kvp.Key, null);
                    boardState.GetOrCreatePaths(kvp.Value, kvp.Key);
                }

            }
        }
    }
}