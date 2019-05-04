using System;
using System.Collections.Generic;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Chess.Entities
{
    public static class ChessPieceEntityFactory
    {
        public const string ValidPieces = "PRNBKQ";
        private static readonly IDictionary<ChessPieceName, Func<Colours, IBoardEntity<ChessPieceName, Colours>>> Factory = new Dictionary<ChessPieceName, Func<Colours, IBoardEntity<ChessPieceName, Colours>>>
        {
            { ChessPieceName.Pawn, (c) => new PawnEntity(c) },
            { ChessPieceName.Knight, (c) => new KnightEntity(c) },
            { ChessPieceName.Bishop, (c) => new BishopEntity(c) },
            { ChessPieceName.Rook, (c) => new RookEntity(c) },
            { ChessPieceName.King, (c) => new KingEntity(c) },
            { ChessPieceName.Queen , (c) => new QueenEntity(c) }
        };

        public static IBoardEntity<ChessPieceName, Colours> Create(ChessPieceName chessPiece, Colours player) => Factory[chessPiece](player);
        public static IBoardEntity<ChessPieceName, Colours> CreatePawn(Colours player)   => Create(ChessPieceName.Pawn,player);
        public static IBoardEntity<ChessPieceName, Colours> CreateBishop(Colours player)  => Create(ChessPieceName.Bishop, player);
        public static IBoardEntity<ChessPieceName, Colours> CreateKing(Colours player)   => Create(ChessPieceName.King, player);
        public static IBoardEntity<ChessPieceName, Colours> CreateKnight(Colours player) => Create(ChessPieceName.Knight, player);
        public static IBoardEntity<ChessPieceName, Colours> CreateQueen(Colours player) => Create(ChessPieceName.Queen, player);
        public static IBoardEntity<ChessPieceName, Colours> CreateRook(Colours player) => Create(ChessPieceName.Rook, player);
    }
}