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
                .Select(l => new ChessMove(at, l, PromotedTo(l, pieceColour, MoveType.Take)))
                );

            moves.AddRange(NormalTakes(board, at, Chess.Board.RightDirectionModifier)
                .Select(l => new ChessMove(at, l, PromotedTo(l, pieceColour, MoveType.Take)))
                );

            moves.AddRange(EnPassantTakes(board, at, Chess.Board.LeftDirectionModifier)
                .Select(l => new ChessMove(at, l, PromotedTo(l, pieceColour, MoveType.TakeEnPassant)))
                );

            moves.AddRange(EnPassantTakes(board, at, Chess.Board.RightDirectionModifier)
                .Select(l => new ChessMove(at, l, PromotedTo(l, pieceColour, MoveType.TakeEnPassant)))
                );

            return moves;
        }

        protected override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var chessPiece = board[at].Piece;
            var direction = Chess.Pieces.VerticalDirectionModifierFor(chessPiece);
            var boardLocation = new BoardLocation(at.File, at.Rank + direction);
            var newMove = new ChessMove(at, boardLocation, PromotedTo(boardLocation, chessPiece.Colour, MoveType.Move));

            var validMoves = new List<ChessMove>();
            if (board.IsEmptyAt(newMove.To))
            {
                validMoves.Add(newMove);
                if (board[at].Location.Rank == Chess.StartingPawnRankFor(chessPiece.Colour) )
                {
                    var location = new BoardLocation(at.File, at.Rank + (direction * 2));
                    newMove = new ChessMove(at, location, PromotedTo(location, chessPiece.Colour, MoveType.Move));

                    if (board.IsEmptyAt(newMove.To))
                        validMoves.Add(newMove);
                }
            }

            return validMoves;
        }

        private MoveType PromotedTo(BoardLocation location, Chess.Colours colour, MoveType dflt)
        {
            return location.Rank == Chess.PromotionRankFor(colour) 
                ? MoveType.Promotion 
                : dflt;
        }
        private IEnumerable<BoardLocation> NormalTakes(ChessBoard board, BoardLocation at, int horizontal)
        {
            var vertical = Chess.Pieces.VerticalDirectionModifierFor(board[at].Piece);

            var pieceColour = board[at].Piece.Colour;

            var moveTos = new List<BoardLocation>();

            if (CanTakeThisSide(at, horizontal))
            {
                var newFile = (int) at.File + horizontal;
                var newRank = at.Rank + vertical;
                var takeLocation = BoardLocation.At(newFile, newRank);

                if (Chess.CanTakeAt(board, takeLocation, pieceColour))
                {
                    moveTos.Add(takeLocation);
                }

                return moveTos;
            }

            return moveTos;
        }

        private IEnumerable<BoardLocation> EnPassantTakes(ChessBoard board, BoardLocation at, int horizontal)
        {
            var vertical = Chess.Pieces.VerticalDirectionModifierFor(board[at].Piece);

            var moveTos = new List<BoardLocation>();

            var enpassantFromRank = Chess.Pieces.EnpassantFromRankFor(board[at].Piece.Colour);

            if (at.Rank == enpassantFromRank)
            {
                if (CanTakeThisSide(at, horizontal))
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

        private static bool CanTakeThisSide(BoardLocation at, int horizontal)
        {
            var notOnHorizontalEdge = horizontal > 0
                ? at.File < Chess.ChessFile.H
                : at.File > Chess.ChessFile.A;
            return notOnHorizontalEdge;
        }
    }
}