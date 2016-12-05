using System;
using System.Collections.Generic;
using CSharpChess.ValidMoves;

namespace CSharpChess.TheBoard
{
    public class ValidMoveFactory
    {
        public ValidMoveFactory()
        {
        }

        public IEnumerable<ChessMove> GetValidMoves(ChessBoard board, BoardLocation at)
        {
            var pieceName = board[at].Piece.Name;
            switch (pieceName)
            {
                case Chess.PieceNames.Pawn:
                    return new PawnValidMoveGenerator().For(board, at);
                case Chess.PieceNames.Knight:
                    return new KnightValidMoveGenerator().For(board, at);
                case Chess.PieceNames.Rook:
                    return new RookValidMoveGenerator().For(board, at);
                case Chess.PieceNames.Bishop:
                    return new BishopValidMoveGenerator().For(board, at);
                case Chess.PieceNames.King:
                    return new KingValidMoveGenerator().For(board, at);
                case Chess.PieceNames.Queen:
                    return new QueenValidMoveGenerator().For(board, at);
//                case Chess.PieceNames.Blank:
//                    break;
                default:
                    throw new NotImplementedException($"ValidMoveGenerator for {pieceName} not yet implemented.");
            }
        }
    }
}