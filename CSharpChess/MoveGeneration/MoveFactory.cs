using System;
using System.Collections.Generic;

namespace CSharpChess.MoveGeneration
{
    public class MoveFactory
    {
        public readonly IDictionary<Chess.Board.PieceNames, Func<IMoveGenerator>> For = new Dictionary<Chess.Board.PieceNames, Func<IMoveGenerator>>
        {
            {Chess.Board.PieceNames.Pawn, () =>new PawnMoveGenerator() },
            {Chess.Board.PieceNames.Knight, () => new KnightMoveGenerator() },
            {Chess.Board.PieceNames.Rook, () => new RookMoveGenerator() },
            {Chess.Board.PieceNames.Bishop, () => new BishopMoveGenerator() },
            {Chess.Board.PieceNames.King, () => new KingMoveGenerator() },
            {Chess.Board.PieceNames.Queen, () => new QueenMoveGenerator() }
        };
    }
}