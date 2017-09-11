using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;

namespace CSharpChess.Movement
{
    public class KingMoveGenerator : MoveGeneratorBase
    {
        protected override IEnumerable<Move> ValidMoves(CSharpChess.Board board, BoardLocation at)
            => AddTransformationsIf(board, at, (b, f, t) => b.IsEmptyAt(t), 
                MoveType.Move, King.MovementTransformations)
            .Concat(Castles(board, at));

        protected override IEnumerable<Move> ValidCovers(CSharpChess.Board board, BoardLocation at) 
            => AddTransformationsIf(board, at, (b, f, t) => board.IsCoveringAt(t, board[f].Piece.Colour), 
                MoveType.Cover, King.MovementTransformations);

        protected override IEnumerable<Move> ValidTakes(CSharpChess.Board board, BoardLocation at) 
            => AddTransformationsIf(board, at, (b, f, t) => b.CanTakeAt(t, b[f].Piece.Colour), 
                MoveType.Take, King.MovementTransformations);

        private IEnumerable<Move> Castles(CSharpChess.Board board, BoardLocation kingLocation)
        {
            var kingPiece = board[kingLocation];
            if (kingPiece.Piece.IsNot(PieceNames.King) || kingPiece.MoveHistory.Any()) return new List<Move>();

            var moves = new List<Move>();

            var queenSideDestination = BoardLocation.At(ChessFile.C, kingLocation.Rank);
            var queenSideRookLocation = BoardLocation.At(ChessFile.A, kingLocation.Rank);

            moves.AddRange(CreateCastleMoveIfAllowed(board, kingLocation, queenSideRookLocation, queenSideDestination));

            var kingSideDestination = BoardLocation.At(ChessFile.G, kingLocation.Rank);
            var kingSideRookLocation = BoardLocation.At(ChessFile.H, kingLocation.Rank);
            moves.AddRange(CreateCastleMoveIfAllowed(board, kingLocation, kingSideRookLocation, kingSideDestination));

            return moves;
        }

        private static List<Move> CreateCastleMoveIfAllowed(CSharpChess.Board board, BoardLocation kingLocation, BoardLocation rookLocation,
            BoardLocation kingDestination)
        {
            var m = new List<Move>();
            if (board[rookLocation].MoveHistory.None())
            {
                if (board.CanCastle(kingDestination))
                {
                    m.Add(new Move(kingLocation, kingDestination, MoveType.Castle));
                }
            }
            return m;
        }
    }
}