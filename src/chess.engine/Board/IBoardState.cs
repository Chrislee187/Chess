using System;
using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Board
{
    // TODO: Want to make this generic, but so much depends on BoardState
    public interface IBoardState<TEntity> : ICloneable where TEntity : class, ICloneable
    {
        void PlaceEntity(BoardLocation loc, TEntity entity);
        LocatedItem<TEntity> GetItem(BoardLocation loc);

        bool IsEmpty(BoardLocation location);

        IEnumerable<LocatedItem<TEntity>> GetItems(params BoardLocation[] locations);
        IEnumerable<LocatedItem<TEntity>> GetItems(ChessPieceName pieceType);
        IEnumerable<LocatedItem<TEntity>> GetItems(Colours colour);
        IEnumerable<LocatedItem<TEntity>> GetItems(Colours colour, ChessPieceName piece);

        void Clear();
        void Remove(BoardLocation loc);

        IEnumerable<BoardLocation> GetAllMoveDestinations(Colours forPlayer);
        IEnumerable<BoardLocation> LocationsOf(Colours owner, ChessPieceName piece);
        IEnumerable<BoardLocation> LocationsOf(Colours owner);
        IEnumerable<BoardLocation> GetAllItemLocations { get; }

        void RegeneratePaths(BoardLocation at);

        void RegeneratePaths(Colours colour);
        void RegenerateAllPaths();
    }
}