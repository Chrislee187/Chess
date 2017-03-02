using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    public class KingMoveGenerator : MoveGeneratorBase
    {
        protected override IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at)
            => AddTransformationsIf(board, at, (b, f, t) => b.IsEmptyAt(t), 
                MoveType.Move, Chess.Rules.King.MovementTransformations)
            .Concat(Castles(board, at));

        protected override IEnumerable<ChessMove> ValidCovers(ChessBoard board, BoardLocation at) 
            => AddTransformationsIf(board, at, (b, f, t) => board.IsCoveringAt(t, board[f].Piece.Colour), 
                MoveType.Cover, Chess.Rules.King.MovementTransformations);

        protected override IEnumerable<ChessMove> ValidTakes(ChessBoard board, BoardLocation at) 
            => AddTransformationsIf(board, at, (b, f, t) => b.CanTakeAt(t, b[f].Piece.Colour), 
                MoveType.Take, Chess.Rules.King.MovementTransformations);

        private IEnumerable<ChessMove> Castles(ChessBoard board, BoardLocation kingLocation)
        {
            var kingPiece = board[kingLocation];
            if (kingPiece.Piece.IsNot(Chess.PieceNames.King) || kingPiece.MoveHistory.Any()) return new List<ChessMove>();

            var moves = new List<ChessMove>();

            var queenSideDestination = BoardLocation.At(Chess.Board.ChessFile.C, kingLocation.Rank);
            var queenSideRookLocation = BoardLocation.At(Chess.Board.ChessFile.A, kingLocation.Rank);

            moves.AddRange(CreateCastleMoveIfAllowed(board, kingLocation, queenSideRookLocation, queenSideDestination));

            var kingSideDestination = BoardLocation.At(Chess.Board.ChessFile.G, kingLocation.Rank);
            var kingSideRookLocation = BoardLocation.At(Chess.Board.ChessFile.H, kingLocation.Rank);
            moves.AddRange(CreateCastleMoveIfAllowed(board, kingLocation, kingSideRookLocation, kingSideDestination));

            return moves;
        }

        private static List<ChessMove> CreateCastleMoveIfAllowed(ChessBoard board, BoardLocation kingLocation, BoardLocation rookLocation,
            BoardLocation kingDestination)
        {
            var m = new List<ChessMove>();
            if (board[rookLocation].MoveHistory.None())
            {
                if (board.CanCastle(kingDestination))
                {
                    m.Add(new ChessMove(kingLocation, kingDestination, MoveType.Castle));
                }
            }
            return m;
        }
    }
}