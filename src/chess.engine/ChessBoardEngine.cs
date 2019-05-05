using System;
using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine
{
    // TODO: This almost fully generic, refactor ChessFile references to ints
    public class ChessBoardEngine<TEntity> where TEntity : class, IBoardEntity
    {
        public readonly IBoardState<TEntity> BoardState;
        private readonly BoardActionFactory<TEntity> _boardActionFactory;

        private readonly IGameSetup<TEntity> _gameSetup;
        private readonly IRefreshAllPaths<TEntity> _allPathCalculator;

        public ChessBoardEngine(IGameSetup<TEntity> gameSetup, IPathsValidator<TEntity> chessPathValidator) : this(gameSetup, chessPathValidator, new DefaultRefreshAllPaths())
        {
        }

        public ChessBoardEngine(IGameSetup<TEntity> gameSetup, IPathsValidator<TEntity> chessPathsValidator, IRefreshAllPaths<TEntity> allPathCalculator)
        {
            _boardActionFactory = new BoardActionFactory<TEntity>();

            BoardState = new BoardState<TEntity>(chessPathsValidator, _boardActionFactory);

            _gameSetup = gameSetup;
            _gameSetup.SetupPieces(this);

            _allPathCalculator = allPathCalculator;
            RefreshAllPaths();
        }

        public void ResetBoard()
        {
            ClearBoard();
            _gameSetup.SetupPieces(this);
            RefreshAllPaths();
        }

        public void ClearBoard() => BoardState.Clear();

        public ChessBoardEngine<TEntity> AddPiece(TEntity create, string startingLocation) 
            => AddPiece(create, BoardLocation.At(startingLocation));
        public ChessBoardEngine<TEntity> AddPiece(TEntity create, BoardLocation startingLocation)
        {
            BoardState.PlaceEntity(startingLocation, create);
            return this;
        }
        
        public LocatedItem<TEntity> PieceAt(string location) => PieceAt((BoardLocation)location);
        public LocatedItem<TEntity> PieceAt(BoardLocation location)
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

                        if (BoardState.IsEmpty(location))
                        {
                            pieces[(int) file - 1, rank - 1] = null;
                        }
                        var entity = BoardState.IsEmpty(location) 
                            ? null 
                            : BoardState.GetItem(location).Item;

                        pieces[(int)file - 1, rank - 1] = entity == null
                            ? null
                            : new BoardPiece((Colours)entity.Owner,(ChessPieceName) Enum.Parse(typeof(ChessPieceName), entity.EntityName));
                    }
                }

                return pieces;
            }
        }

        //TODO: Need an abstraction around MoveType's and Actions, to not be Chess specific
        // so will need some default state (move entity, remove entity) but can be extended with custom ones, (enpassant, castle)
        public void Move(BoardMove move)
        {
            var action = _boardActionFactory.Create(move.MoveType, BoardState);

            action.Execute(move);

            _allPathCalculator.RefreshAllPaths(BoardState, true);

        }

        private void RefreshAllPaths()
        {
            _allPathCalculator.RefreshAllPaths(BoardState, true);
        }

        private class DefaultRefreshAllPaths : IRefreshAllPaths<TEntity>
        {
            public void RefreshAllPaths(IBoardState<TEntity> boardState, bool removeMovesThatLeaveKingInCheck)
            {
                foreach (var loc in boardState.GetAllItemLocations)
                {
                    boardState.GeneratePaths(boardState.GetItem(loc).Item, loc);
                }
            }
        }
    }
}