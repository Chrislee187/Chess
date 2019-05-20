using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace board.engine.Board
{
    public class BoardState<TEntity> : IBoardState<TEntity> where TEntity : class, IBoardEntity
    {
        public static bool ParalleiseRefreshAllPaths => FeatureFlags.ParalleliseRefreshAllPaths;

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
            if (ParalleiseRefreshAllPaths)
            {
                RegenerateAllPathsParallel();
            }
            else
            {
                RegenerateAllPathsSequential();
            }
        }

        private void RegenerateAllPathsSequential()
        {
            foreach (var loc in GetAllItemLocations)
            {
                RegeneratePaths(loc);
            }
        }

        public void RegenerateAllPathsParallel()
        {
            // new ways - this one is abit slower not sure why
            // 175-190% first observed values | 250% (avg 5 runs)
            // Parallel.ForEach(GetAllItemLocations, RegeneratePaths);

            // 215-221% first observed values | 275% (avg 5 runs)
            GetAllItemLocations.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism) // This seems to give small improvement, bigger test sample needed
                .ForAll(RegeneratePaths);

            // Old way
            // 220% first observed values | 280% (avg 5 runs)
            // var tasks = new List<Task>();
            // foreach (var loc in GetAllItemLocations)
            // {
            //     tasks.Add(Task.Run(() => {
            //         RegeneratePaths(loc);
            //     }));
            // 
            // }
            // Task.WaitAll(tasks.ToArray());
        }

        public void RegeneratePaths(int owner)
        {
            GetItems(owner).AsParallel()
                .ForAll(i => RegeneratePaths(i.Location));
        }

        #region For DEBUGGING use, remove in due course

        public string ToTextBoard()
        {
            var sb = new StringBuilder();
            for (var rank = 7; rank >= 0; rank--)
            {
                for (var file = 0; file < 8; file++)
                {
                    var entity = GetItem(BoardLocation.At(file+1, rank+1))?.Item;
                    char chr;
                    if (entity == null)
                    {
                        chr = '.';
                    }
                    else
                    {
                        chr = ChessPieceNameMapper.ToChar(entity.EntityType, entity.Owner);
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
                {'k', 5},
                {'K', 5},
                {'q', 6},
                {'Q', 6},
            };

            public static bool ContainsPiece(char p)
            {
                return PieceNames.ContainsKey(p);
            }

            public static int FromChar(char c)
            {
                return PieceNames[c];
            }

            public static int ToOwner(char c)
            {
                return char.IsUpper(c) ? 0 : 1;
            }

            public static char ToChar(int piece, int owner)
            {
                var c = PieceNames.FirstOrDefault(n => n.Value == piece).Key;

                return owner == 0 ? char.ToUpper(c) : char.ToLower(c);
            }
        }

        #endregion

    }
}