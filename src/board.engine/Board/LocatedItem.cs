using System;
using board.engine.Movement;

namespace board.engine.Board
{
    public class LocatedItem<TEntity> : ICloneable where TEntity : class, ICloneable, IBoardEntity
    {
        public LocatedItem(BoardLocation location, TEntity item, Paths paths)
        {
            Location = location;
            Item = item;
            UpdatePaths(paths);
        }

        public BoardLocation Location { get; }
        public TEntity Item { get; }
        public Paths Paths { get; private set; }

        public void UpdatePaths(Paths paths)
        {
            Paths = paths ?? new Paths();
        }

        public object Clone()
        {
            return new LocatedItem<TEntity>(
                Location.Clone() as BoardLocation, 
                Item.Clone() as TEntity, 
                Paths.Clone() as Paths);
        }
    }
}