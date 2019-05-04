using System;
using System.Collections.Generic;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Chess.Entities
{
    public static class ChessPieceEntityFactory
    {
        public const string ValidPieces = "PRNBKQ";
        private static readonly IDictionary<ChessPieceName, Func<Colours, ChessPieceEntity>> Factory 
            = new Dictionary<ChessPieceName, Func<Colours, ChessPieceEntity>>
        {
            { ChessPieceName.Pawn, (c) => new PawnEntity(c) },
            { ChessPieceName.Knight, (c) => new KnightEntity(c) },
            { ChessPieceName.Bishop, (c) => new BishopEntity(c) },
            { ChessPieceName.Rook, (c) => new RookEntity(c) },
            { ChessPieceName.King, (c) => new KingEntity(c) },
            { ChessPieceName.Queen , (c) => new QueenEntity(c) }
        };

        public static ChessPieceEntity Create(ChessPieceName chessPiece, Colours player) => Factory[chessPiece](player);
        public static ChessPieceEntity Create(ChessPieceName chessPiece, object player) => Factory[chessPiece]((Colours) player);
        public static ChessPieceEntity CreatePawn(Colours player)   => Create(ChessPieceName.Pawn,player);
        public static ChessPieceEntity CreateBishop(Colours player)  => Create(ChessPieceName.Bishop, player);
        public static ChessPieceEntity CreateKing(Colours player)   => Create(ChessPieceName.King, player);
        public static ChessPieceEntity CreateKnight(Colours player) => Create(ChessPieceName.Knight, player);
        public static ChessPieceEntity CreateQueen(Colours player) => Create(ChessPieceName.Queen, player);
        public static ChessPieceEntity CreateRook(Colours player) => Create(ChessPieceName.Rook, player);
    }
}