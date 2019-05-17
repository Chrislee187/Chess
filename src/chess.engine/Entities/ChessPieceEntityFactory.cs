using System;
using System.Collections.Generic;
using board.engine;
using chess.engine.Game;

namespace chess.engine.Entities
{
    public class ChessPieceEntityFactory : IBoardEntityFactory<ChessPieceEntity>
    {
        public string ValidPieces { get; }= "PRNBKQ";
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

        public ChessPieceEntity Create(object typeData)
        {
            var data = typeData as ChessPieceEntityFactoryTypeExtraData;
            if (data == null)
            {
                throw new ArgumentException($"{nameof(typeData)} is not of type {nameof(ChessPieceEntityFactoryTypeExtraData)}");

            }
            return Create(data.PieceName, data.Owner);
        }

        public ChessPieceEntity Create(ChessPieceName pieceName, Colours colour)
        {

            return Factory[pieceName](colour);
        }
        public class ChessPieceEntityFactoryTypeExtraData
        {
            public ChessPieceName PieceName { get; set; }
            public Colours Owner { get; set; }
        }
//        public static ChessPieceEntity Create(ChessPieceName chessPiece, Colours player) => ActionProvider[chessPiece](player);
//        public static ChessPieceEntity CreatePawn(Colours player)   => Create(ChessPieceName.Pawn,player);
//        public static ChessPieceEntity CreateBishop(Colours player)  => Create(ChessPieceName.Bishop, player);
//        public static ChessPieceEntity CreateKing(Colours player)   => Create(ChessPieceName.King, player);
//        public static ChessPieceEntity CreateKnight(Colours player) => Create(ChessPieceName.Knight, player);
//        public static ChessPieceEntity CreateQueen(Colours player) => Create(ChessPieceName.Queen, player);
//        public static ChessPieceEntity CreateRook(Colours player) => Create(ChessPieceName.Rook, player);
    }
}