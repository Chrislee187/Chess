using System;
using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    // TODO: Want to make this generic, but so much depends on BoardState
    public interface IBoardState/*<TEntity, TOwner>*/ : ICloneable
    {
        void PlaceEntity(BoardLocation loc, IBoardEntity<ChessPieceName, Colours> entity, bool generateMoves = true);
        LocatedItem<IBoardEntity<ChessPieceName, Colours>> GetItem(BoardLocation loc);

        bool IsEmpty(BoardLocation location);
        bool DoesMoveLeaveMovingPlayersKingInCheck(BoardMove move);

        IEnumerable<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> GetItems(params BoardLocation[] locations);
        IEnumerable<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> GetItems(ChessPieceName pieceType);
        IEnumerable<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> GetItems(Colours colour);
        IEnumerable<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> GetItems(Colours colour, ChessPieceName piece);

        void Clear();
        void Remove(BoardLocation loc);

        IEnumerable<BoardLocation> GetAllMoveDestinations(Colours forPlayer);
        IEnumerable<BoardLocation> LocationsOf(Colours player, ChessPieceName piece);
        IEnumerable<BoardLocation> LocationsOf(Colours player);
        IEnumerable<BoardLocation> GetAllItemLocations { get; }

        void GeneratePaths(IBoardEntity<ChessPieceName, Colours> forEntity, BoardLocation at, bool removeMovesThatLeaveKingInCheck = true);
//        Paths GeneratePossiblePaths(ChessPieceEntity entity, BoardLocation boardLocation);
        GameState CheckForCheckMate(Colours forPlayer, List<LocatedItem<IBoardEntity<ChessPieceName, Colours>>> enemiesAttackingKing);
        GameState CurrentGameState(Colours forPlayer);
    }
}