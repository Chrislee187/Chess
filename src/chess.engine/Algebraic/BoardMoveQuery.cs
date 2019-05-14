using System;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess;
using chess.engine.Chess.Entities;
using chess.engine.Game;

namespace chess.engine.Algebraic
{
    // WIP
    public class BoardMoveQuery
    {
        public int MoveType { get; private set; } = -1;
        public ChessPieceName? PromotionPiece { get; private set; }
        public ChessPieceName Piece { get; private set; }
        public Colours Owner{ get; private set; }
        public int FromFile { get; private set; } = -1;
        public int FromRank { get; private set; } = -1;

        public int ToFile { get; private set; } = -1;
        public int ToRank { get; private set; } = -1;

        public bool QueryResolved => !ChessGame.OutOfBounds(FromFile)
                                     && !ChessGame.OutOfBounds(FromRank)
                                     && !ChessGame.OutOfBounds(ToFile)
                                     && !ChessGame.OutOfBounds(ToRank)
                                     && MoveType != -1;

        public bool GameOver { get; private set; }
//        public ChessGameResult GameResult { get; private set; }
        public string PgnText { get; private set; } = string.Empty;

        private int ParseFile(char file)
        {
            int fileChar = file.ToString().ToUpper().First();
            int result = fileChar - 64; // Assuming ascii value 65 for 'A'

            if (ChessGame.OutOfBounds(result))
            {
                throw new ArgumentOutOfRangeException(nameof(file), $"Invalid rank {file}");
            }

            return result;
        }

        private int ParseRank(char rank)
        {
            if (!int.TryParse(rank.ToString().ToUpper(), out var result))
            {
                throw new ArgumentOutOfRangeException(nameof(rank), $"Invalid rank {rank}");
            }
            if (ChessGame.OutOfBounds(result))
            {
                throw new ArgumentOutOfRangeException(nameof(rank), $"Invalid rank {rank}");
            }

            return result;
        }

        public void WithToFile(char file) => ToFile = ParseFile(file);
        public void WithFromFile(char file) => FromFile = ParseFile(file);
        public void WithToRank(char rank) => ToRank = ParseRank(rank);
        public void WithFromRank(char rank) => FromRank = ParseRank(rank);
        public void WithColour(Colours turn) => Owner = turn;
        public void WithPiece(Colours owner, ChessPieceName piece)
        {
            Owner = owner;
            Piece = piece;
        }

        public void WithMoveType(SanMoveTypes moveType) => MoveType = (int) moveType;

        public void WithResult(string move)
        {
            ToFile = -1;
            ToRank = -1;
            FromFile = -1;
            FromRank = -1;
            Piece = GetPieceMoveANMove(move);

//            GameResult = PgnResult.Parse(move);
            GameOver = true;
        }

        private ChessPieceName GetPieceMoveANMove(string move)
        {
            return ChessPieceName.Pawn; // TODO: Can derive this from the string directly
        }

        private BoardMove CreateMove()
        {
            var from = BoardLocation.At(FromFile, FromRank);
            var to = BoardLocation.At(ToFile, ToRank);
            var move = new BoardMove(from, to, MoveType, PromotionPiece);
            return move;
        }

        public void WithPromotion(char promotionPiece)
        {
            PromotionPiece = GetPromotionPiece(promotionPiece.ToString());
        }
        private static ChessPieceName GetPromotionPiece(string piece)
        {
            switch (piece.ToUpper())
            {
                case "R": return ChessPieceName.Rook;
                case "B": return ChessPieceName.Bishop;
                case "N": return ChessPieceName.Knight;
                case "Q": return ChessPieceName.Queen;
            }

            throw new ArgumentException($"'{piece}' is not a valid promotion", nameof(piece));

        }
        public string ToMove()
        {
            return $"{CreateMove()}";
        }

        public void WithPgn(string pgnmovetext)
        {
            PgnText = pgnmovetext;
        }
    }
}