using System;
using System.Collections.Generic;

namespace CSharpChess.MoveGeneration
{
    public class MoveFactory
    {
        public readonly IDictionary<Chess.PieceNames, Func<IMoveGenerator>> For = new Dictionary<Chess.PieceNames, Func<IMoveGenerator>>
        {
            {Chess.PieceNames.Pawn, () =>new PawnMoveGenerator() },
            {Chess.PieceNames.Knight, () => new KnightMoveGenerator() },
            {Chess.PieceNames.Rook, () => new RookMoveGenerator() },
            {Chess.PieceNames.Bishop, () => new BishopMoveGenerator() },
            {Chess.PieceNames.King, () => new KingMoveGenerator() },
            {Chess.PieceNames.Queen, () => new QueenMoveGenerator() }
        };
    }
}