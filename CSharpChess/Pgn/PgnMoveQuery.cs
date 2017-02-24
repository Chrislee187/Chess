using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CSharpChess.Helpers;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.Pgn
{
    /*
     * Pgn notation requires current board state to determine the actual moveType
     * as it is not explicit about the piece being moved only what happened and
     * where the destination was.
     * 
     * We will parse the raw text to create a pgnMoveQuery to apply against the board
     * that can be used to determine the missing details
     * 
     */

    public class PgnMoveQuery
    {
        public delegate BoardLocation PgnQuery(ChessBoard board, Chess.Colours turn, Chess.PieceNames pieceName, BoardLocation move);

        private static readonly IDictionary<char, Chess.PieceNames> PieceNameMap = new Dictionary<char, Chess.PieceNames>
        {
            {'P', Chess.PieceNames.Pawn },
            {'N', Chess.PieceNames.Knight },
            {'B', Chess.PieceNames.Bishop },
            {'R', Chess.PieceNames.Rook },
            {'Q', Chess.PieceNames.Queen },
            {'K', Chess.PieceNames.King },
            {'O', Chess.PieceNames.King },
        };

        public static bool TryParse(Chess.Colours turn, string move, out MoveQuery moveQuery)
        {
            var success = false;
            var fromFile = Chess.Board.ChessFile.None;
            moveQuery = new MoveQuery();
            moveQuery.SetColour(turn);

            if (move.EndsWith("+"))
            {
                move = move.Substring(0, move.Length - 1);
                if (!TryParse(turn, move, out moveQuery)) return false;

                // TODO: If the sub-parse returns 'Check' probably checkmate
                moveQuery.SetMoveType(MoveType.Check);
                return true;
            }

            if (move.Length == 2)
            {
                MoveLength2(turn, move, moveQuery);
                return true;
            }

            if (move.Length == 3)
            {
                MoveLength3(turn, move, moveQuery);
                return true;
            }
            if (move.Length == 4)
            {
                MoveLength4(turn, move, moveQuery);
                return true;
            }

            if (move.ToUpper() == "O-O-O")
            {
                CreateCastleMoveQuery(turn, move, moveQuery);
                return true;
            }

            if (move.Contains("-"))
            {
                moveQuery.SetResult(move);
                return true;
            }
            return false;
        }

        private static void CreateCastleMoveQuery(Chess.Colours turn, string move, MoveQuery moveQuery)
        {
            var dest = CalcKingDestinationForCastle(turn, move);
            moveQuery.SetMoveType(MoveType.Castle);
            moveQuery.ResolveFromFile('e');
            moveQuery.ResolveFromRank(turn == Chess.Colours.White ? '1' : '8');
            moveQuery.ResolveToFile(dest[0]);
            moveQuery.ResolveToRank(dest[1]);
        }

        private static void MoveLength4(Chess.Colours turn, string move, MoveQuery moveQuery)
        {
/*
                    Nbd7
                    Nxe4
                    Ra6+
                    cxb5
                 */
            moveQuery.SetMoveType(TheBoard.MoveType.Move);

            if (char.IsUpper(move[0]))
            {
                moveQuery.SetPiece(new ChessPiece(turn, GetPieceName(move[0])));
            }
            else
            {
                moveQuery.SetPiece(new ChessPiece(turn, Chess.PieceNames.Pawn));
                moveQuery.ResolveFromFile(move[0]);
            }

            if (move[1].ToString().ToLower() == "x")
            {
                moveQuery.SetMoveType(TheBoard.MoveType.Take);
                moveQuery.ResolveToFile(move[2]);
                moveQuery.ResolveToRank(move[3]);
            }
            else
            {
                moveQuery.ResolveFromFile(move[1]);
                moveQuery.ResolveToFile(move[2]);
                moveQuery.ResolveToRank(move[3]);
            }
        }

        private static void MoveLength3(Chess.Colours turn, string move, MoveQuery moveQuery)
        {
            var pieceName = GetPieceName(move[0]);
            moveQuery.SetPiece(new ChessPiece(turn, pieceName));
            if (move[0].ToString().ToUpper() == "O")
            {
                CreateCastleMoveQuery(turn, move, moveQuery);
            }
            else
            {
                moveQuery.SetMoveType(MoveType.Move);
                moveQuery.ResolveToFile(move[1]);
                moveQuery.ResolveToRank(move[2]);
            }
        }

        private static void MoveLength2(Chess.Colours turn, string move, MoveQuery moveQuery)
        {
            moveQuery.SetPiece(new ChessPiece(turn, Chess.PieceNames.Pawn));
            moveQuery.SetMoveType(TheBoard.MoveType.Move);
            moveQuery.ResolveFromFile(move[0]);
            moveQuery.ResolveToFile(move[0]);
            moveQuery.ResolveToRank(move[1]);
        }

        private static string CalcKingDestinationForCastle(Chess.Colours turn, string move)
        {
            if (move.ToUpper() == "O-O")
            {
                return "G" + ((turn == Chess.Colours.White) ? "1" : "8");
            }

            return "C" + ((turn == Chess.Colours.White) ? "1" : "8");
        }

        private static Chess.PieceNames GetPieceName(char chr)
        {
            Chess.PieceNames result = Chess.PieceNames.Blank;
            
            if (char.IsUpper(chr))
            {
                var pieceChar = char.ToUpper(chr);
                if (!PieceNameMap.ContainsKey(pieceChar))
                {
                    throw new NotImplementedException($"'{pieceChar}' is not a valid SAN piece.");
                }
                result = PieceNameMap[pieceChar];
            }
            else
            {
                result = Chess.PieceNames.Pawn;
            }
            return result;
        }
    }

    public class MoveQuery
    {
        private MoveType _moveType;
        private Chess.Colours _turn;
        public ChessPiece Piece { get; private set; }
        public Chess.Board.ChessFile FromFile{ get; private set; } = Chess.Board.ChessFile.None;
        public int FromRank { get; private set; } = 0;

        public Chess.Board.ChessFile ToFile{ get; private set; } = Chess.Board.ChessFile.None;
        public int ToRank { get; private set; } = 0;

        public bool QueryResolved
        {
            get
            {
                return !Chess.Board.Validations.InvalidRank(FromRank)
                       && !Chess.Board.Validations.InvalidFile(FromFile)
                       && !Chess.Board.Validations.InvalidRank(ToRank)
                       && !Chess.Board.Validations.InvalidFile(ToFile)
                       || GameOver;
            }
        }

        public bool GameOver { get; private set; }

        private Chess.Board.ChessFile ParseFile(char file)
        {
            Chess.Board.ChessFile test;
            if (Enum.TryParse(file.ToString().ToUpper(), out test))
            {
                return test;
            }

            throw new ArgumentOutOfRangeException(nameof(file), "Invalid file");
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

        public void ResolveToFile(char file)
        {
            ToFile = ParseFile(file);
        }

        public void ResolveFromFile(char file)
        {
            FromFile = ParseFile(file);
        }

        public void ResolveToRank(char rank)
        {
            ToRank = ParseRank(rank);
        }

        public void ResolveFromRank(char rank)
        {
            FromRank = ParseRank(rank);
        }

        public void SetColour(Chess.Colours turn)
        {
            _turn = turn;
        }

        public void SetPiece(ChessPiece chessPiece)
        {
            Piece = chessPiece;
        }

        public void SetMoveType(MoveType moveType)
        {
            _moveType = moveType;
        }

        public void ResolveWithBoard(ChessBoard chessBoard)
        {
            var bl = FindPieceThatCanMoveTo(chessBoard, _turn, Piece.Name, BoardLocation.At(ToFile, ToRank));

            FromFile = bl.File;
            FromRank = bl.Rank;
        }

        private BoardLocation FindPieceThatCanMoveTo(ChessBoard board, Chess.Colours turn, Chess.PieceNames pieceName, BoardLocation move)
        {
            var boardPieces = board.Pieces.OfColour(turn).Where(p => p.Piece.Name.Equals(pieceName));
            if (FromFile != Chess.Board.ChessFile.None)
            {
                boardPieces = boardPieces.Where(p => p.Location.File == FromFile);
            }

            var pieces = boardPieces.Where(p => p.PossibleMoves.ContainsMoveTo(move)).ToList();
            var piece = pieces.First();

            if (pieces.Count() > 1)
            {
                piece = pieces.SingleOrDefault(p => p.PossibleMoves.Any(pm => pm.MoveType == _moveType));
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
            if (GameOver) return "end";

            var from = new BoardLocation(FromFile, FromRank);
            var to = new BoardLocation(ToFile, ToRank);
            var move = new ChessMove(from,to, MoveType.Move);
            return move.ToString();
        }

        public void SetResult(string move)
        {
            ToFile = Chess.Board.ChessFile.None;
            ToRank = 0;
            FromFile = Chess.Board.ChessFile.None;
            FromRank = 0;
            GameOver = true;
        }
    }
}