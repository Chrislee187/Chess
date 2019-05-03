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
            _allPathCalculator.RefreshAllPaths(BoardState);
        }

        public void ClearBoard() => BoardState.Clear();

        public ChessBoardEngine AddPiece(ChessPieceEntity create, string startingLocation) => AddPiece(create, BoardLocation.At(startingLocation));
        public ChessBoardEngine AddPiece(ChessPieceEntity create, BoardLocation startingLocation)
        {
            BoardState.PlaceEntity(startingLocation, create);
            return this;
        }
        
        public LocatedItem<ChessPieceEntity> PieceAt(string location) => PieceAt((BoardLocation)location);
        public LocatedItem<ChessPieceEntity> PieceAt(BoardLocation location)
        {
            if (BoardState.IsEmpty(location)) return null;
            
            var piece = BoardState.GetItem(location);

            return piece;
        }

        public BoardPiece[,] Board
        {
            get
            {
                var pieces = new BoardPiece[8, 8];
                foreach (ChessFile file in Enum.GetValues(typeof(ChessFile)))
                {
                    for (var rank = 8; rank > 0; rank--)
                    {
                        var location = BoardLocation.At(file, rank);

                        var entity = BoardState.IsEmpty(location) 
                            ? null 
                            : BoardState.GetItem(location).Item;

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
        ChessPieceEntity IBoardState.GetEntity(BoardLocation loc) 
            => BoardState.IsEmpty(loc) ? null : BoardState.GetItem(loc).Item;

        void IBoardState.SetEntity(BoardLocation loc, ChessPieceEntity entity)
        {
            BoardState.PlaceEntity(loc, entity, BoardState.GeneratePossiblePaths(entity, loc));
        }

        void IBoardState.ClearLocation(BoardLocation loc)
        {
            BoardState.Remove(loc);
        }
        #endregion

        private class DefaultRefreshAllPaths : IRefreshAllPaths
        {
            public void RefreshAllPaths(BoardState boardState)
            {
                foreach (var loc in boardState.LocationsInUse)
                {
                    boardState.UpdatePaths(boardState.GetItem(loc).Item, loc);
                }

            }
        }
    }
}