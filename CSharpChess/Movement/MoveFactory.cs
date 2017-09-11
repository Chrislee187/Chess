using System;
using System.Collections.Generic;

namespace CSharpChess.Movement
{
    public static class MoveFactory
    {
        public static readonly IDictionary<PieceNames, Func<IMoveGenerator>> For = new Dictionary<PieceNames, Func<IMoveGenerator>>
        {
            {PieceNames.Pawn, () =>new PawnMoveGenerator() },
            {PieceNames.Knight, () => new KnightMoveGenerator() },
            {PieceNames.Rook, () => new RookMoveGenerator() },
            {PieceNames.Bishop, () => new BishopMoveGenerator() },
            {PieceNames.King, () => new KingMoveGenerator() },
            {PieceNames.Queen, () => new QueenMoveGenerator() }
        };
    }
}