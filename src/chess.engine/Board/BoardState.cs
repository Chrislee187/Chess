using System.Collections.Generic;
using System.Linq;
using chess.engine.Actions;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Board
{
    public class BoardState<TEntity> : IBoardState<TEntity> where TEntity : class, IBoardEntity
    {
        private readonly IDictionary<BoardLocation, LocatedItem<TEntity>> _items;
        private readonly IBoardActionFactory<TEntity> _actionFactory;
        private readonly IPathsValidator<TEntity> _pathsValidator;

        public BoardState(IPathsValidator<TEntity> pathsValidator, 
            IBoardActionFactory<TEntity> actionFactory) : this(pathsValidator, actionFactory, null)
        {
        }

        private BoardState(IPathsValidator<TEntity> pathsValidator, 
            IBoardActionFactory<TEntity> actionFactory, 
            IEnumerable<LocatedItem<TEntity>> clonedItems) 
        {
            _items = clonedItems?.ToDictionary(k => k.Location, k => k) 
                     ?? new Dictionary<BoardLocation, LocatedItem<TEntity>>();
            _pathsValidator = pathsValidator;
            _actionFactory = actionFactory;
        }


        public void PlaceEntity(BoardLocation loc, TEntity entity)
            => _items[loc] = new LocatedItem<TEntity>(loc, entity, null);

        public IEnumerable<BoardLocation> GetAllItemLocations => _items.Keys;

        public LocatedItem<TEntity> GetItem(BoardLocation loc)
            => GetItems(loc).SingleOrDefault();

        public IEnumerable<LocatedItem<TEntity>> GetItems(params BoardLocation[] locations)
            => _items.Where(itm => locations.Contains(itm.Key)).Select(kvp => kvp.Value);

        public IEnumerable<LocatedItem<TEntity>> GetItems(ChessPieceName pieceType)
            => _items.Values.Where(itm => itm.Item.EntityName == pieceType.ToString());

        public IEnumerable<LocatedItem<TEntity>> GetItems(Colours owner)
            => _items.Where(itm => itm.Value.Item.Owner.Equals(owner)).Select(kvp => kvp.Value);

        public IEnumerable<LocatedItem<TEntity>> GetItems(Colours owner, ChessPieceName piece) =>
            _items.Where(itm => itm.Value.Item.Owner.Equals(owner)
                                && itm.Value.Item.EntityName == piece.ToString()
            ).Select(kvp => kvp.Value);

        public void Remove(BoardLocation loc) => _items.Remove(loc);
        public void Clear() => _items.Clear();

        public object Clone()
        {
            var clonedItems = _items.Values.Select(e => e.Clone() as LocatedItem<TEntity>);

            var clonedState = new BoardState<TEntity>(_pathsValidator, _actionFactory, clonedItems);

            return clonedState;
        }

        // TODO: This should not be here, pull in to something with a simple dependency on the IBoardState interface
        public GameState CurrentGameState(Colours currentPlayer, Colours enemy)
        {
            var items = GetEnemiesAttackingKing(currentPlayer).ToList();
            if (items.Any())
            {
                return CheckForCheckMate(currentPlayer, items);
            }

            return GameState.InProgress;
        }

        // TODO: Pull out of board state, probably need some form of ChessGameState component we can passin!
        private IEnumerable<LocatedItem<TEntity>> GetEnemiesAttackingKing(Colours kingColour)
        {
            var king = GetItems(kingColour, ChessPieceName.King).First();

            var locatedItems = GetItems(kingColour.Enemy());

            var enemiesAttackingKing = locatedItems.Where(itm
                => itm.Paths.ContainsMoveTo(king.Location)).ToList();
            return enemiesAttackingKing;
        }

        private GameState CheckForCheckMate(Colours forPlayer, IEnumerable<LocatedItem<TEntity>> enemiesAttackingKing)
        {
            // TODO: Pull this out to test
            var state = GameState.Check;
            var king = GetItems(forPlayer, ChessPieceName.King).Single();
            var kingCannotMove = !king.Paths.Any(); // Move validator will ensure we can't move into check

            var friendlyDestinations = GetAllMoveDestinations(forPlayer);

            bool canBlock = enemiesAttackingKing.All(enemy =>
            {
                var attackingPath = enemy.Paths
                    .Single(attackPath => attackPath.Any(p => p.To.Equals(king.Location)));

                // Check if any friendly pieces can move to the path or take the item
                return friendlyDestinations.Any(fd => fd.Equals(enemy.Location)
                                                      || attackingPath.Any(move => move.To.Equals(fd))
                );
            });

            if (kingCannotMove && !canBlock)
            {
                state = GameState.Checkmate;
            }

            return state;
        }

        // TODO: ChessPieceName, Colours are not generic
        public IEnumerable<BoardLocation> LocationsOf(Colours owner, ChessPieceName piece)
        {
            return _items
                .Where(kvp => kvp.Value.Item.EntityName.Equals(piece.ToString())
                              && kvp.Value.Item.Owner.Equals(owner))
                .Select(kvp => kvp.Key);
        }

        public IEnumerable<BoardLocation> LocationsOf(Colours owner)
        {
            return _items
                .Where(kvp => kvp.Value.Item.Owner.Equals(owner))
                .Select(kvp => kvp.Key);
        }

        public bool IsEmpty(BoardLocation location) => !_items.ContainsKey(location);

        public void RegeneratePaths(BoardLocation at)
        {
            var item = GetItem(at);

            Guard.NotNull(item, $"Null item found at {at}!");

            var paths = _pathsValidator.GeneratePossiblePaths(this, item.Item, at);

            item.UpdatePaths(paths);
        }

        public void RegenerateAllPaths()
        {
            foreach (var loc in GetAllItemLocations)
            {
                RegeneratePaths(loc);
            }
        }
        public void RegeneratePaths(Colours colour)
        {
            foreach (var enemyPiece in GetItems(colour))
            {
                RegeneratePaths(enemyPiece.Location);
            }
        }
        
        public IEnumerable<BoardLocation> GetAllMoveDestinations(Colours forPlayer)
        {
            var friendlyItems = GetItems(forPlayer).Where(i => !i.Item.EntityName.Equals(ChessPieceName.King.ToString()));
            var friendlyDestinations = friendlyItems.SelectMany(fi => fi.Paths.FlattenMoves()).Select(m => m.To);
            return friendlyDestinations;
        }
    }

}