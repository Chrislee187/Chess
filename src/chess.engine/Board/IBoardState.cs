using System;
using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    // TODO: Want to make this generic, but so much depends on BoardState
    public interface IBoardState<TEntity> : ICloneable
    {
        void PlaceEntity(BoardLocation loc, TEntity entity, bool generateMoves = true);
        LocatedItem<TEntity> GetItem(BoardLocation loc);

        bool IsEmpty(BoardLocation location);
        bool DoesMoveLeaveMovingPlayersKingInCheck(BoardMove move);

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

        void GeneratePaths(TEntity forEntity, BoardLocation at, bool removeMovesThatLeaveKingInCheck = true);
//        Paths GeneratePossiblePaths(ChessPieceEntity entity, BoardLocation boardLocation);
        GameState CheckForCheckMate(Colours forPlayer, List<LocatedItem<TEntity>> enemiesAttackingKing);
        GameState CurrentGameState(Colours currentPlayer, Colours enemy);
    }
}