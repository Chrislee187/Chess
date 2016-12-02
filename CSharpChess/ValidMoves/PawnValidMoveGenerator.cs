using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class PawnValidMoveGenerator
    {
        public IEnumerable<ChessMove> For(ChessBoard board, BoardLocation at)
        {
            var possibleMoves = new List<ChessMove>();

            possibleMoves.AddRange(Moves(board, at));

            possibleMoves.AddRange(Takes(board, at));

            // TODO: Promotions
            
            return possibleMoves;
        }

        private IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            var moves = new List<ChessMove>();

            moves.AddRange(NormalTakeMoves(board, at, Chess.Board.LeftDirectionModifier)
                .Select(l => new ChessMove(at, l, MoveType.Take)));
            moves.AddRange(NormalTakeMoves(board, at, Chess.Board.RightDirectionModifier)
                .Select(l => new ChessMove(at, l, MoveType.Take)));

            moves.AddRange(EnPassantTakeMoves(board, at, Chess.Board.LeftDirectionModifier)
                .Select(l => new ChessMove(at, l, MoveType.TakeEnPassant)));
            moves.AddRange(EnPassantTakeMoves(board, at, Chess.Board.RightDirectionModifier)
                .Select(l => new ChessMove(at, l, MoveType.TakeEnPassant)));

            return moves;
        }

        private IEnumerable<BoardLocation> NormalTakeMoves(ChessBoard board, BoardLocation at, int horizontal)
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

            if (CanTakeAt(board, takeLocation, pieceColour))
            {
                moveTos.Add(takeLocation);
            }

            return moveTos;
        }

        private IEnumerable<BoardLocation> EnPassantTakeMoves(ChessBoard board, BoardLocation at, int horizontal)
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
                    var moveLocation = new BoardLocation((Chess.ChessFile)newFile, at.Rank + vertical);

                    if (EnPassantAvailable(board, at, moveLocation))
                    {
                        moveTos.Add(moveLocation);
                    }
                }
            }
            
            return moveTos;
        }

        private static bool EnPassantAvailable(ChessBoard board, BoardLocation at, BoardLocation moveLocation)
        {
            var newFile = moveLocation.File;
            var takeLocation = new BoardLocation(newFile, at.Rank);
            var piece = board[takeLocation].Piece;
            var canTakeAPiece = board.IsNotEmptyAt(takeLocation)
                                && piece.Is(Chess.PieceNames.Pawn)
                                && piece.IsNot(board[at].Piece.Colour)
                                && board[takeLocation].MoveHistory.Count() == 1
                ;
            var moveToSpotIsVacant = board.IsEmptyAt(moveLocation);

            return (canTakeAPiece && moveToSpotIsVacant);
        }

        private static bool CanTakeAt(ChessBoard board, BoardLocation takeLocation, Chess.Colours colourOfTakingPiece) 
            => !board.IsEmptyAt(takeLocation) && board[takeLocation].Piece.Colour != colourOfTakingPiece;

        private static IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            if(chessPiece.Colour == Chess.Colours.None) return new List<ChessMove>();

            var direction = Chess.Pieces.Direction(chessPiece);

            var newMove = new ChessMove(at, new BoardLocation(at.File, at.Rank + direction), MoveType.Move);

            var validMoves = new List<ChessMove>();
            if (!Blocked(board, newMove))
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == Chess.StartingPawnRankFor(chessPiece.Colour) )
                {
                    newMove = new ChessMove(at, new BoardLocation(at.File, at.Rank + (direction * 2)), MoveType.Move);
                    if (!Blocked(board, newMove))
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }

        private static bool Blocked(ChessBoard board, ChessMove chessMove)
        {
            return !board[chessMove.To].Piece.Equals(ChessPiece.NullPiece);
        }
    }
}