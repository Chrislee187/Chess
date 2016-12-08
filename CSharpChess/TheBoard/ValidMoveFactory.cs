using System;
using System.Collections.Generic;
using CSharpChess.ValidMoves;

namespace CSharpChess.TheBoard
{
    public class ValidMoveFactory
    {
        public readonly IDictionary<Chess.PieceNames, ValidMoveGeneratorBase> For = new Dictionary<Chess.PieceNames, ValidMoveGeneratorBase>
        {
            {Chess.PieceNames.Pawn, new PawnValidMoveGenerator() },
            {Chess.PieceNames.Knight, new KnightValidMoveGenerator() },
            {Chess.PieceNames.Rook, new RookValidMoveGenerator() },
            {Chess.PieceNames.Bishop, new BishopValidMoveGenerator() },
            {Chess.PieceNames.King, new KingValidMoveGenerator() },
            {Chess.PieceNames.Queen, new QueenValidMoveGenerator() }
        };

        public IEnumerable<ChessMove> GetValidMoves(ChessBoard board, BoardLocation at)
        {
            var pieceName = board[at].Piece.Name;

            if (For.ContainsKey(pieceName))
                return For[pieceName].ValidMoves(board, at);

            throw new NotImplementedException($"ValidMoveGenerator for {pieceName} not yet implemented.");
        }
        public IEnumerable<BoardLocation> GetThreateningMoves(ChessBoard board, BoardLocation at)
        {
            var pieceName = board[at].Piece.Name;

            if (For.ContainsKey(pieceName))
                return For[pieceName].ValidThreats(board, at);

            throw new NotImplementedException($"ValidMoveGenerator for {pieceName} not yet implemented.");
        }
    }
}