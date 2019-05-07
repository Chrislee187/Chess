using System;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    public class LocatedItem<T> : ICloneable where T : class, ICloneable, IBoardEntity
    {
        public LocatedItem(BoardLocation location, T item, Paths paths)
        {
            Location = location;
            Item = item;
            UpdatePaths(paths);
        }

        public BoardLocation Location { get; }
        public T Item { get; }
        public Paths Paths { get; private set; }

        public void UpdatePaths(Paths paths)
        {
            Paths = paths ?? new Paths();
        }

        public object Clone()
        {
            return new LocatedItem<T>(
                Location.Clone() as BoardLocation, 
                Item.Clone() as T, 
                Paths.Clone() as Paths);
        }
    }
}