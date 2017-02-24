using System;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.Pgn
{
    public class PgnQuery
    {
        private MoveType _moveType;
        private Chess.Colours _turn;
        public ChessPiece Piece { get; private set; }
        public Chess.Board.ChessFile FromFile{ get; private set; } = Chess.Board.ChessFile.None;
        public int FromRank { get; private set; } = 0;

        public Chess.Board.ChessFile ToFile{ get; private set; } = Chess.Board.ChessFile.None;
        public int ToRank { get; private set; } = 0;

        public bool QueryResolved => !Chess.Board.Validations.InvalidRank(FromRank)
                                     && !Chess.Board.Validations.InvalidFile(FromFile)
                                     && !Chess.Board.Validations.InvalidRank(ToRank)
                                     && !Chess.Board.Validations.InvalidFile(ToFile)
                                     || GameOver;

        public bool GameOver { get; private set; }

        private Chess.Board.ChessFile ParseFile(char file)
        {
            Chess.Board.ChessFile test;
            if (Enum.TryParse(file.ToString().ToUpper(), out test))
            {
                return test;
            }

            throw new ArgumentOutOfRangeException(nameof(file), $"Invalid file: {file}");
        }

        private int ParseRank(char rank)
        {
            int test;
            if (!int.TryParse(rank.ToString().ToUpper(), out test))
            {
                throw new ArgumentOutOfRangeException(nameof(rank), "Invalid file");
            }

            if (Chess.Board.Validations.InvalidRank(test))
            {
                throw new ArgumentOutOfRangeException(nameof(rank), "Invalid file");
            }

            return test;
        }

        public void WithToFile(char file) => ToFile = ParseFile(file);
        public void WithFromFile(char file) => FromFile = ParseFile(file);
        public void WithToRank(char rank) => ToRank = ParseRank(rank);
        public void WithFromRank(char rank) => FromRank = ParseRank(rank);
        public void WithColour(Chess.Colours turn) => _turn = turn;
        public void WithPiece(ChessPiece chessPiece) => Piece = chessPiece;
        public void WithMoveType(MoveType moveType) => _moveType = moveType;

        public void WithResult(string move)
        {
            ToFile = Chess.Board.ChessFile.None;
            ToRank = 0;
            FromFile = Chess.Board.ChessFile.None;
            FromRank = 0;

            if (move == "1/2-1/2")
            {

            }
            GameOver = true;
        }

        public void ResolveQuery(ChessBoard chessBoard)
        {
            var bl = FindPieceThatCanMoveTo(chessBoard, _turn, Piece.Name, BoardLocation.At(ToFile, ToRank));

            FromFile = bl.File;
            FromRank = bl.Rank;
        }

        private BoardLocation FindPieceThatCanMoveTo(ChessBoard board, Chess.Colours turn, Chess.PieceNames pieceName, BoardLocation move)
        {
            var boardPiecesQuery = board.Pieces.Where(p => p.Piece.Is(turn, pieceName));

            if (FromFile != Chess.Board.ChessFile.None)
            {
                boardPiecesQuery = boardPiecesQuery.Where(p => p.Location.File == FromFile);
            }

            boardPiecesQuery = boardPiecesQuery.Where(p => p.PossibleMoves.ContainsMoveTo(move));

            var boardPieces = boardPiecesQuery.ToList();
            var piece = boardPieces.First();

            if (boardPieces.Count() > 1)
            {
                piece = boardPieces.SingleOrDefault(p => p.PossibleMoves.Any(pm => pm.MoveType == _moveType));
            }

            if (piece == null)
            {
                Console.WriteLine(board.ToAsciiBoard());
                throw new InvalidOperationException($"No {pieceName} that can {_moveType} to {move} found");
            }

            return piece.Location;
        }

        public override string ToString()
        {
            if (GameOver) return "end"; // TODO: End game handling in the pgn parser

            var from = new BoardLocation(FromFile, FromRank);
            var to = new BoardLocation(ToFile, ToRank);
            var move = new ChessMove(from,to, MoveType.Move);
            return move.ToString();
        }

        public void WithPromotion(char promotionPiece)
        {
        }
    }
}