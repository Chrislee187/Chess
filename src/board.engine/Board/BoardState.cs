using System.Collections.Generic;
using System.Linq;

namespace board.engine.Board
{
    public class BoardState<TEntity> : IBoardState<TEntity> where TEntity : class, IBoardEntity
    {
        private readonly IDictionary<BoardLocation, LocatedItem<TEntity>> _items;
        private readonly IPathsValidator<TEntity> _pathsValidator;

        public BoardState(IPathsValidator<TEntity> pathsValidator
            ) : this(pathsValidator, null)
        {
        }

        private BoardState(IPathsValidator<TEntity> pathsValidator, 
            IEnumerable<LocatedItem<TEntity>> clonedItems) 
        {
            _items = clonedItems?.ToDictionary(k => k.Location, k => k) 
                     ?? new Dictionary<BoardLocation, LocatedItem<TEntity>>();
            _pathsValidator = pathsValidator;
        }

        public void PlaceEntity(BoardLocation loc, TEntity entity)
            => _items[loc] = new LocatedItem<TEntity>(loc, entity, null);

        public IEnumerable<BoardLocation> GetAllItemLocations => _items.Keys;

        public LocatedItem<TEntity> GetItem(BoardLocation loc)
            => GetItems(loc).SingleOrDefault();

        public IEnumerable<LocatedItem<TEntity>> GetItems(params BoardLocation[] locations)
            => _items.Where(itm => locations.Contains(itm.Key)).Select(kvp => kvp.Value);

        public IEnumerable<LocatedItem<TEntity>> GetItems(int owner)
            => _items.Values.ForOwner(owner);

        public IEnumerable<LocatedItem<TEntity>> GetItems(int owner, int entityType) =>
            _items.Where(itm => itm.Value.Item.Owner.Equals(owner)
                                && itm.Value.Item.EntityType == entityType
            ).Select(kvp => kvp.Value);

        public IEnumerable<LocatedItem<TEntity>> GetItems()
            => _items.Values;

        public void Remove(BoardLocation loc) => _items.Remove(loc);
        public void Clear() => _items.Clear();

        public object Clone()
        {
            var clonedItems = _items.Values.Select(e => e.Clone() as LocatedItem<TEntity>);

            var clonedState = new BoardState<TEntity>(_pathsValidator, clonedItems);

            return clonedState;
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
        public void RegeneratePaths(int owner)
        {
            foreach (var enemyPiece in GetItems(owner))
            {
                RegeneratePaths(enemyPiece.Location);
            }
        }
    }
}