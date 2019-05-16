using System.Linq;
using board.engine.Actions;
using board.engine.Board;
using board.engine.Movement;
using Microsoft.Extensions.Logging;

namespace board.engine
{
    public class BoardEngine<TEntity> where TEntity : class, IBoardEntity
    {
        public readonly IBoardState<TEntity> BoardState;
        private readonly IBoardActionProvider<TEntity> _boardActionProvider;

        private readonly IBoardSetup<TEntity> _boardSetup;
        private readonly IRefreshAllPaths<TEntity> _refreshAllPaths;
        private ILogger<BoardEngine<TEntity>> _logger;
        private IBoardActionProvider<TEntity> _actionProvider;

        public int Width { get; private set; } = 8;
        public int Height { get; private set; } = 8;



        public BoardEngine(
            ILogger<BoardEngine<TEntity>> logger,
            IBoardSetup<TEntity> boardSetup, 
            IPathsValidator<TEntity> pathsValidator,
            IBoardActionProvider<TEntity> actionProvider
            )

            : this(logger, boardSetup, pathsValidator, actionProvider, new DefaultRefreshAllPaths())
        {
        }

        public BoardEngine(
            ILogger<BoardEngine<TEntity>> logger, 
            IBoardSetup<TEntity> boardSetup,
            IPathsValidator<TEntity> pathsValidator,
            IBoardActionProvider<TEntity> actionProvider,
            IRefreshAllPaths<TEntity> refreshAllPaths)
        {
            _actionProvider = actionProvider;
            _logger = logger;
            _boardActionProvider = actionProvider;

            BoardState = new BoardState<TEntity>(pathsValidator, _boardActionProvider);

            _boardSetup = boardSetup;
            _boardSetup.SetupPieces(this);

            _refreshAllPaths = refreshAllPaths;
            _refreshAllPaths.RefreshAllPaths(BoardState);
        }

        public void ResetBoard()
        {
            ClearBoard();
            _boardSetup.SetupPieces(this);
            _refreshAllPaths.RefreshAllPaths(BoardState);
        }

        public void ClearBoard() => BoardState.Clear();

        public BoardEngine<TEntity> AddPiece(TEntity create, BoardLocation startingLocation)
        {
            BoardState.PlaceEntity(startingLocation, create);
            return this;
        }

        public LocatedItem<TEntity> PieceAt(BoardLocation location)
        {
            if (BoardState.IsEmpty(location)) return null;
            
            var piece = BoardState.GetItem(location);

            return piece;
        }

        public LocatedItem<TEntity>[,] Board
        {
            get
            {
                var pieces = new LocatedItem<TEntity>[8, 8];
                for (int x = 1; x <= Width; x++)
                {
                    for (var y = Height; y > 0; y--)
                    {
                        var location = BoardLocation.At(x, y);

                        if (BoardState.IsEmpty(location))
                        {
                            pieces[x - 1, y - 1] = null;
                        }
                        else
                        {
                            var entity = BoardState.GetItem(location);
                            pieces[x - 1, y - 1] = entity;
                        }
                    }
                }

                return pieces;
            }
        }

        public void Move(BoardMove move)
        {
            var action = _boardActionProvider.Create((int) move.MoveType, BoardState);

            action.Execute(move);

            _refreshAllPaths.RefreshAllPaths(BoardState);
        }

        private class DefaultRefreshAllPaths : IRefreshAllPaths<TEntity>
        {
            public void RefreshAllPaths(IBoardState<TEntity> boardState) 
                => boardState.GetAllItemLocations.ToList()
                    .ForEach(boardState.RegeneratePaths);
        }
    }
}