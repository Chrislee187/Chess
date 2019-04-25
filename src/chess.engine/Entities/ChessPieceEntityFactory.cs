using System;
using System.Collections.Generic;
using chess.engine.Game;

namespace chess.engine.Entities
{
    public static class ChessPieceEntityFactory
    {
        private static IDictionary<ChessPieceName, Func<Colours, ChessPieceEntity>> _factory = new Dictionary<ChessPieceName, Func<Colours, ChessPieceEntity>>
        {
            {
                ChessPieceName.Pawn, (c) => new PawnEntity(c)
            }
        };
        public static ChessPieceEntity Create(ChessPieceName chessPiece, Colours player)
        {
            return _factory[chessPiece](player);
        }
    }
}