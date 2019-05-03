using System;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    public class LocatedItem<T> 
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
    }
}