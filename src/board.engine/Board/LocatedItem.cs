using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

    public static class LocatedItemExtensions {

        public static BoardMove FindMoveTo<TEntity>(this LocatedItem<TEntity> item, BoardLocation destination)
            where TEntity : class, IBoardEntity
        {
            return item.Paths.FlattenMoves()
                .SingleOrDefault(m => m.To.Equals(destination));
        }


        public static IEnumerable<LocatedItem<TEntity>>
            ThatCanMoveTo<TEntity>(
                this IEnumerable<LocatedItem<TEntity>> items,
                BoardLocation location)
            where TEntity : class, IBoardEntity
        {
            return items.Where(itm
                => itm.Paths.ContainsMoveTo(location));
        }

        public static IEnumerable<LocatedItem<TEntity>>
            ThatCanMoveTypeTo<TEntity>(
                this IEnumerable<LocatedItem<TEntity>> items,
                BoardLocation location,
                params int[] moveTypesAndActions)
            where TEntity : class, IBoardEntity
        {
            return items.Where(itm
                => itm.Paths.ContainsMoveTypeTo(location, moveTypesAndActions));
        }

        public static IEnumerable<LocatedItem<TEntity>>
            ForOwner<TEntity>(
                this IEnumerable<LocatedItem<TEntity>> items,
                int owner)
            where TEntity : class, IBoardEntity
        {
            return items.Where(itm => itm.Item.Owner == owner);
        }

        public static IEnumerable<BoardLocation>
            AllDestinations<TEntity>(
                this IEnumerable<LocatedItem<TEntity>> items)
            where TEntity : class, IBoardEntity
        {
            return items
                .SelectMany(fi => fi.Paths.FlattenMoves())
                .Select(m => m.To);
        }

    }
}