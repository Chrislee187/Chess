using System;
using System.Collections.Generic;

namespace board.engine.Board
{
    public interface IBoardState<TEntity> : ICloneable where TEntity : class, IBoardEntity
    {
        void PlaceEntity(BoardLocation loc, TEntity entity);
        LocatedItem<TEntity> GetItem(BoardLocation loc);

        bool IsEmpty(BoardLocation location);

        IEnumerable<LocatedItem<TEntity>> GetItems(params BoardLocation[] locations);
        IEnumerable<LocatedItem<TEntity>> GetItems(int owner);
        IEnumerable<LocatedItem<TEntity>> GetItems(int owner, int entityType);
        IEnumerable<LocatedItem<TEntity>> GetItems();

        void Clear();
        void Remove(BoardLocation loc);
        IEnumerable<BoardLocation> GetAllItemLocations { get; }

        void RegenerateAllPaths();
        void RegeneratePaths(int owner);
        void RegeneratePaths(BoardLocation at);
    }
}