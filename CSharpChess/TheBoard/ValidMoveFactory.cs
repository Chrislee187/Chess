using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.ValidMoves;

namespace CSharpChess.TheBoard
{
    public class ValidMoveFactory
    {
        public readonly IDictionary<Chess.Board.PieceNames, ValidMoveGeneratorBase> For = new Dictionary<Chess.Board.PieceNames, ValidMoveGeneratorBase>
        {
            {Chess.Board.PieceNames.Pawn, new PawnValidMoveGenerator() },
            {Chess.Board.PieceNames.Knight, new KnightValidMoveGenerator() },
            {Chess.Board.PieceNames.Rook, new RookValidMoveGenerator() },
            {Chess.Board.PieceNames.Bishop, new BishopValidMoveGenerator() },
            {Chess.Board.PieceNames.King, new KingValidMoveGenerator() },
            {Chess.Board.PieceNames.Queen, new QueenValidMoveGenerator() }
        };

        public IEnumerable<ChessMove> GetValidMoves(ChessBoard board, BoardLocation at)
        {
            var pieceName = board[at].Piece.Name;

            if (For.ContainsKey(pieceName))
                return For[pieceName].All(board, at).Where(m => m.MoveType.IsMove() || m.MoveType.IsTake());

            throw new NotImplementedException($"ValidMoveGenerator for {pieceName} not yet implemented.");
        }
    }
}