using System.Linq;
using chess.engine.Actions;
using chess.engine.Board;
using chess.engine.Game;
using chess.engine.Movement;
using Microsoft.Extensions.Logging;

namespace chess.engine
{
    // TODO: This almost fully generic for any kind of "entity" we want to put on a chess-like board
    // * refactor to not be Colours
    // * refactor next player logic to be external

    public class BoardEngine<TEntity> where TEntity : class, IBoardEntity
    {
        public readonly IBoardState<TEntity> BoardState;
        private readonly BoardActionFactory<TEntity> _boardActionFactory;

        private readonly IGameSetup<TEntity> _gameSetup;
        private readonly IRefreshAllPaths<TEntity> _allPathCalculator;
        private ILogger<BoardEngine<TEntity>> _logger;

        public int Width { get; private set; } = 8;
        public int Height { get; private set; } = 8;



        public BoardEngine(
            ILogger<BoardEngine<TEntity>> logger,
            IGameSetup<TEntity> gameSetup, 
            IPathsValidator<TEntity> pathsValidator) 
            : this(logger, gameSetup, pathsValidator, new DefaultRefreshAllPaths())
        {
        }

        public BoardEngine(
            ILogger<BoardEngine<TEntity>> logger, 
            IGameSetup<TEntity> gameSetup,
            IPathsValidator<TEntity> pathsValidator,
            IRefreshAllPaths<TEntity> allPathCalculator)
        {
            _logger = logger;
            _boardActionFactory = new BoardActionFactory<TEntity>();

            BoardState = new BoardState<TEntity>(pathsValidator, _boardActionFactory);

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

        public BoardEngine<TEntity> AddPiece(TEntity create, string startingLocation) 
            => AddPiece(create, BoardLocation.At(startingLocation));
        public BoardEngine<TEntity> AddPiece(TEntity create, BoardLocation startingLocation)
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

        //TODO: Need an abstraction around MoveType's and Actions, to not be Chess specific
        // so will need some default types (move entity, remove entity) but can be extended with custom ones, (enpassant, castle)
        public void Move(BoardMove move)
        {
            var action = _boardActionFactory.Create(move.MoveType, BoardState);

            action.Execute(move);

            _allPathCalculator.RefreshAllPaths(BoardState);
        }

        private class DefaultRefreshAllPaths : IRefreshAllPaths<TEntity>
        {
            public void RefreshAllPaths(IBoardState<TEntity> boardState) 
                => boardState.GetAllItemLocations.ToList().ForEach(boardState.RegeneratePaths);
        }
    }
}