using System;
using System.Linq;
using System.Text;
using board.engine;
using board.engine.Board;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Pieces;
using Microsoft.Extensions.DependencyModel;

namespace chess.engine.Game
{
    public class ChessBoardBuilder
    {
        private const string ValidPieces = "PKQRNB .E";
        private readonly char[,] _board = new char[8,8];

        public ChessBoardBuilder Rank(int rank, string pieces)
        {
            CheckValidPieces(pieces);

            CheckValidRank(rank);

            var file = 0;

            foreach (var piece in pieces)
            {
                if (ValidPieces.Contains(piece.ToString().ToUpper()))
                {
                    _board[file++, rank-1] = piece;
                }
                else
                {
                    throw new Exception($"Don't know how to map '{piece}' for board building purposes");
                }
            }

            return this;
        }

        public ChessBoardBuilder File(ChessFile chessFile, string pieces)
        {
            CheckValidPieces(pieces);

            var rank = 0;

            foreach (var piece in pieces)
            {
                if (ValidPieces.Contains(piece.ToString().ToUpper()))
                {
                    _board[(int) chessFile -1, rank++] = piece;
                }
            }

            return this;
        }

        private static void CheckValidRank(int rank) =>
            Guard.ArgumentException(() => rank < 1 || rank > 8,
                $"{nameof(rank)} must be in the range 1-8");

        private static void CheckValidPieces(string pieces) =>
            Guard.ArgumentException(() => pieces.Length > 8,
                $"{nameof(pieces)} cannot be greater than EIGHT characters");

        public ChessBoardBuilder At(ChessFile file, int rank, char piece)
        {
            CheckValidPieces(piece.ToString());

            CheckValidRank(rank);

            _board[(int)file -1, rank -1] = piece;

            return this;
        }

        public ChessBoardBuilder Board(string boardPieces)
        {
            Guard.ArgumentException(() => boardPieces.Length != 64,
                $"{nameof(boardPieces)} must be 64 char's in length.");

            var ranks = boardPieces.SplitInParts(8);

            var rankIdx = 8;
            foreach (var rank in ranks)
            {
                Rank(rankIdx--, rank);
            }

            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var rank = 7; rank >=0; rank--)
            {
                for (var file = 0; file < 8; file++)
                {
                    var chr = _board[file, rank];
                    sb.Append(chr == '\0' ? '.' : chr);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public ChessBoardBuilder FromChessGame(ChessGame chessGame)
        {
            for (var rank = 7; rank >= 0; rank--)
            {
                for (var file = 0; file < 8; file++)
                {
                    var piece = chessGame.Board[file, rank];

                    if (piece == null)
                    {
                        _board[file, rank] = '.';
                    }
                    else
                    {
                        // TODO: Stop using ToString()/ToUpper() and create a proper abstraction to convert to a single char
                        var entity = piece.Item;
                        var c = entity.Piece == ChessPieceName.Knight ? 'N' : entity.Piece.ToString().First();
                        if (entity.Player == Colours.Black) c = c.ToString().ToLower().First();
                        _board[file, rank] = c;
                    }
                }
            }

            return this;
        }

        public IBoardSetup<ChessPieceEntity> ToGameSetup()
        {
            return new CustomChessBoardSetup(_board);
        }

        public IBoardState<ChessPieceEntity> ToBoardState()
        {
            return ChessFactory.CustomChessGame(ToGameSetup()).BoardState;
        }
        private class CustomChessBoardSetup : IBoardSetup<ChessPieceEntity>
        {
            private readonly char[,] _board;
            private readonly ChessPieceEntityFactory _chessPieceEntityFactory = ChessFactory.ChessPieceEntityFactory();

            public CustomChessBoardSetup(char[,] board)
            {
                _board = board;
            }
            public void SetupPieces(BoardEngine<ChessPieceEntity> engine)
            {
                for (var rank = 7; rank >= 0; rank--)
                {
                    for (var file = 0; file < 8; file++)
                    {
                        var chr = _board[file, rank];

                        if (_chessPieceEntityFactory.ValidPieces.Contains(chr.ToString().ToUpper()))
                        {
                            var entity = _chessPieceEntityFactory.Create(
                                ChessPieceNameMapper.FromChar(chr),
                                char.IsUpper(chr) ? Colours.White : Colours.Black
                            );

                            var location = BoardLocation.At(file + 1, rank + 1);
                            if (chr.ToString().ToUpper() == "E")
                            {
                                // Special pawn char for custom board setups, sets the move history so enpassant validation works
                                entity.AddMoveTo(location);
                            }
                            engine.AddPiece(entity, location);
                        }
                        else if (chr != ' ' && chr != '.' && chr != '\0')
                        {
                            throw new Exception($"Invalid piece '{chr}'");
                        }
                    }
                }
            }
        }
    }
}