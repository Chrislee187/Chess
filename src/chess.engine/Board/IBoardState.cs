using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Entities;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Board
{
    public interface IBoardState
    {
        void PlaceEntity(BoardLocation loc, ChessPieceEntity entity, bool generateMoves = true);
        LocatedItem<ChessPieceEntity> GetItem(BoardLocation loc);

        bool IsEmpty(BoardLocation location);
        bool DoesMoveLeaveMovingPlayersKingInCheck(ChessMove move);

        IEnumerable<LocatedItem<ChessPieceEntity>> GetItems(params BoardLocation[] locations);
        IEnumerable<LocatedItem<ChessPieceEntity>> GetItems(ChessPieceName pieceType);
        IEnumerable<LocatedItem<ChessPieceEntity>> GetItems(Colours colour);
        IEnumerable<LocatedItem<ChessPieceEntity>> GetItems(Colours colour, ChessPieceName piece);

        void Clear();
        void Remove(BoardLocation loc);

        IEnumerable<BoardLocation> GetAllMoveDestinations(Colours forPlayer);
        IEnumerable<BoardLocation> LocationsOf(Colours player, ChessPieceName piece);
        IEnumerable<BoardLocation> LocationsOf(Colours player);
        IEnumerable<BoardLocation> GetAllItemLocations { get; }

        void GeneratePaths(ChessPieceEntity forEntity, BoardLocation at, bool removeMovesThatLeaveKingInCheck = true);
//        Paths GeneratePossiblePaths(ChessPieceEntity entity, BoardLocation boardLocation);
        GameState CheckForCheckMate(Colours forPlayer, List<LocatedItem<ChessPieceEntity>> enemiesAttackingKing);
        GameState CurrentGameState(Colours forPlayer);
    }
}