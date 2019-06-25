using System.Collections.Generic;
using System.Linq;
using System.Text;
using board.engine.Movement;

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

        public LocatedItem<TEntity> GetItem(BoardLocation loc)
        {
            if (_items.TryGetValue(loc, out var value))
                return value;

            return null;
        }
        public IEnumerable<LocatedItem<TEntity>> GetItems(params BoardLocation[] locations)
            => _items.Where(itm => locations.Contains(itm.Key)).Select(kvp => kvp.Value);
        public IEnumerable<LocatedItem<TEntity>> GetItems()
            => _items.Values;
        public IEnumerable<LocatedItem<TEntity>> GetItems(int owner) => _items.Values.ForOwner(owner);
        public IEnumerable<LocatedItem<TEntity>> GetItems(int owner, int entityType) =>
            _items.Where(itm => itm.Value.Item.Owner.Equals(owner)
                                && itm.Value.Item.EntityType == entityType
            ).Select(kvp => kvp.Value);

        public void Remove(BoardLocation loc) => _items.Remove(loc);
        public void Clear() => _items.Clear();
        public object Clone()
        {
            // NOTE: Do not parallise the Values.Select() it makes the clone slower!
            var clonedState = new BoardState<TEntity>(
                _pathsValidator, 
                _items.Values.Select(item => item.Clone() as LocatedItem<TEntity>).ToList());

            return clonedState;
        }

        public bool IsEmpty(BoardLocation location) => !_items.ContainsKey(location);

        public void RegenerateValidatedPaths(LocatedItem<TEntity> locatedItem)
        {
            Guard.NotNull(locatedItem, $"Null item found!");

            var paths = _pathsValidator.GetValidatedPaths(this, locatedItem.Item, locatedItem.Location);

            locatedItem.UpdatePaths(paths);
        }

        public void UpdatePaths(LocatedItem<TEntity>[] items)
        {
            foreach (var item in items)
            {
                _items[item.Location].UpdatePaths((Paths) item.Paths.Clone());
            }
        }


        public void RefreshPathsFor(IEnumerable<LocatedItem<TEntity>> items)
        {
            if (FeatureFlags.ParalleliseRefreshAllPaths)
            {
                items.AsParallel()
                    .ForAll(RegenerateValidatedPaths);
            }
            else
            {
                items.ToList()
                    .ForEach(RegenerateValidatedPaths);
            }
        }

        public void RegenerateValidatedPaths(int owner)
            => RefreshPathsFor(AllItems(owner));

        public void RegenerateValidatedPaths()
        {
            RefreshPathsFor(AllItems());
        }

        private IEnumerable<LocatedItem<TEntity>> AllItems() => _items.Values;

        private IEnumerable<LocatedItem<TEntity>> AllItems(int owner)
            => _items.Values.Where(i => i.Item.Owner == owner);

        #region For DEBUGGING use, remove in due course

        public string ToTextBoard()
        {
            var sb = new StringBuilder();
            for (var rank = 7; rank >= 0; rank--)
            {
                for (var file = 0; file < 8; file++)
                {
                    var loc = BoardLocation.At(file+1, rank+1);
                    var entity = GetItem(loc)?.Item;
                    char chr;
                    if (entity == null)
                    {
                        chr = '.';
                    }
                    else
                    {
                        chr = ChessPieceNameMapper.ToChar(entity.EntityType, entity.Owner);

                       var epY = char.IsUpper(chr) ? 5 : 4;
                        if (chr == 'p' || chr == 'P' && loc.Y == epY && entity.LocationHistory.Count() == 2)
                        {
                            chr = chr == 'p' ? 'e' : 'E';
                        }
                    }

                    sb.Append(chr == '\0' ? '.' : chr);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        //NOTE: Reproduced here for debugging convienced
        static class ChessPieceNameMapper
        {
            private static readonly IDictionary<char, int> PieceNames = new Dictionary<char, int>
            {
                {'p', 1},
                {'P', 1},
                {'r', 2},
                {'R', 2},
                {'n', 4},
                {'N', 4},
                {'b', 3},
                {'B', 3},
                {'k', int.MaxValue},
                {'K', int.MaxValue},
                {'q', 6},
                {'Q', 6},
            };

            public static char ToChar(int piece, int owner)
            {
                var c = PieceNames.FirstOrDefault(n => n.Value == piece).Key;

                return owner == 0 ? char.ToUpper(c) : char.ToLower(c);
            }
        }

        #endregion
    }
}