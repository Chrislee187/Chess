using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class PawnValidMoveGenerator : ValidMoveGeneratorBase
    {
        public PawnValidMoveGenerator() : base(Chess.PieceNames.Pawn)
        {}

        protected override IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();
            var pieceColour = board[at].Piece.Colour;
            
            moves.AddRange(NormalTakes(board, at, Chess.Board.LeftDirectionModifier)
                .Select(l => new ChessMove(at, l, Promotable(l, pieceColour, MoveType.Take)))
                );

            moves.AddRange(NormalTakes(board, at, Chess.Board.RightDirectionModifier)
                .Select(l => new ChessMove(at, l, Promotable(l, pieceColour, MoveType.Take)))
                );

            moves.AddRange(EnPassantTakes(board, at, Chess.Board.LeftDirectionModifier)
                .Select(l => new ChessMove(at, l, Promotable(l, pieceColour, MoveType.TakeEnPassant)))
                );

            moves.AddRange(EnPassantTakes(board, at, Chess.Board.RightDirectionModifier)
                .Select(l => new ChessMove(at, l, Promotable(l, pieceColour, MoveType.TakeEnPassant)))
                );

            return moves;
        }

        protected override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            if (chessPiece.Colour == Chess.Colours.None) return new List<ChessMove>();

            var direction = Chess.Pieces.Direction(chessPiece);
            var boardLocation = new BoardLocation(at.File, at.Rank + direction);

            var newMove = new ChessMove(at, boardLocation, Promotable(boardLocation, chessPiece.Colour, MoveType.Move));

            var validMoves = new List<ChessMove>();
            if (!Blocked(board, newMove))
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == Chess.StartingPawnRankFor(chessPiece.Colour) )
                {
                    var location = new BoardLocation(at.File, at.Rank + (direction * 2));
                    newMove = new ChessMove(at, location, Promotable(location, chessPiece.Colour, MoveType.Move));
                    if (!Blocked(board, newMove))
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }

        private MoveType Promotable(BoardLocation location, Chess.Colours colour, MoveType dflt)
        {
            var promotionRank = Chess.PromotionRankFor(colour);
            return location.Rank == promotionRank ? MoveType.Promotion : dflt;
        }
        private IEnumerable<BoardLocation> NormalTakes(ChessBoard board, BoardLocation at, int horizontal)
        {
            var vertical = Chess.Pieces.Direction(board[at].Piece);

            var pieceColour = board[at].Piece.Colour;
            var notOnHorizontalEdge = horizontal > 0 
                ? at.File < Chess.ChessFile.H 
                : at.File > Chess.ChessFile.A;

            var moveTos = new List<BoardLocation>();

            if (!notOnHorizontalEdge) return moveTos;

            var newFile = (int)at.File + horizontal;
            var newRank = at.Rank + vertical;
            var takeLocation = BoardLocation.At(newFile, newRank);

            if (Chess.CanTakeAt(board, takeLocation, pieceColour))
            {
                moveTos.Add(takeLocation);
            }

            return moveTos;
        }

        private IEnumerable<BoardLocation> EnPassantTakes(ChessBoard board, BoardLocation at, int horizontal)
        {
            var vertical = Chess.Pieces.Direction(board[at].Piece);

            var moveTos = new List<BoardLocation>();

            var enpassantFromRank = Chess.Pieces.EnpassantFromRankFor(board[at].Piece.Colour);

            if (at.Rank == enpassantFromRank)
            {
                var notOnHorizontalEdge = horizontal > 0
                    ? at.File < Chess.ChessFile.H
                    : at.File > Chess.ChessFile.A;

                if (notOnHorizontalEdge)
                {
                    var newFile = (int)at.File + horizontal;
                    var enPassantLocation = new BoardLocation((Chess.ChessFile)newFile, at.Rank + vertical);

                    if (board.CanEnPassant(at, enPassantLocation))
                    {
                        moveTos.Add(enPassantLocation);
                    }
                }
            }
            
            return moveTos;
        }

        private static bool Blocked(ChessBoard board, ChessMove chessMove)
        {
            return !board[chessMove.To].Piece.Equals(ChessPiece.NullPiece);
        }
    }
}