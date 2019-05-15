using System;
using System.Diagnostics;
using board.engine.Movement;

namespace board.engine.Board
{
    [DebuggerDisplayAttribute("{DebuggerDisplayText}")]
    public class LocatedItem<TEntity> : ICloneable where TEntity : class, ICloneable, IBoardEntity
    {
        private string DebuggerDisplayText => $"{Location} {Item.EntityName} ({Item.Owner})";
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