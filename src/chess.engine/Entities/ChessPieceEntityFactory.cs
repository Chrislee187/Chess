using System;
using System.Collections.Generic;
using chess.engine.Game;

namespace chess.engine.Entities
{
    public static class ChessPieceEntityFactory
    {
        private static IDictionary<ChessPieceName, Func<Colours, ChessPieceEntity>> _factory = new Dictionary<ChessPieceName, Func<Colours, ChessPieceEntity>>
        {
            { ChessPieceName.Pawn, (c) => new PawnEntity(c) },
            { ChessPieceName.Knight, (c) => new KnightEntity(c) },
            { ChessPieceName.Bishop, (c) => new BishopEntity(c) },
            { ChessPieceName.Rook, (c) => new RookEntity(c) },
            { ChessPieceName.King, (c) => new KingEntity(c) }
        };
        public static ChessPieceEntity Create(ChessPieceName chessPiece, Colours player)
        {
            return _factory[chessPiece](player);
        }
        public static ChessPieceEntity CreatePawn(Colours player)   => _factory[ChessPieceName.Pawn](player);
        public static ChessPieceEntity CreateBishop(Colours player)  => _factory[ChessPieceName.Bishop](player);
        public static ChessPieceEntity CreateKing(Colours player)   => _factory[ChessPieceName.King](player);
        public static ChessPieceEntity CreateKnight(Colours player) => _factory[ChessPieceName.Knight](player);
//        public static ChessPieceEntity CreateQueen(Colours player) => _factory[ChessPieceName.Queen](player);
        public static ChessPieceEntity CreateRook(Colours player) => _factory[ChessPieceName.Rook](player);
    }
}