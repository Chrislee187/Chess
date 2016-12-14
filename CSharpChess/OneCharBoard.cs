using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess
{
    public class OneCharBoard
    {
        public IEnumerable<string> Ranks { get; }

        public static Chess.Board.Colours PieceColour(char p)
        {
            if(p =='.' || p ==' ') return Chess.Board.Colours.None;

            return char.IsLower(p)
                ? Chess.Board.Colours.Black
                : Chess.Board.Colours.White;
        }

        public static Chess.Board.PieceNames PieceName(char p)
        {
            if(p =='.' || p ==' ') return Chess.Board.PieceNames.Blank;

            var valid = OneCharPieceNames.Where(c => char.ToUpper(p) == c.Value).ToList();
            if (valid.None())
                throw new ArgumentException($"Invalid One Char representations of a chess piece found '{p}'", nameof(p));

            var pieceNames = OneCharPieceNames.First(c => char.ToUpper(p) == c.Value).Key;
//            Console.WriteLine($"{pieceNames} == {p}");
            return pieceNames;
        }

        public static char ToChar(BoardPiece p)
        {
            var c = OneCharPieceNames[p.Piece.Name].ToString();
            if (p.Piece.Colour == Chess.Board.Colours.Black) c = c.ToLower();
            return c[0];
        }

        public static ChessPiece ToChessPiece(char c) => new ChessPiece(PieceColour(c), PieceName(c));

        public OneCharBoard(ChessBoard board)
        {
            var ranks = new string[8];
            var id = 1;
            foreach (var boardRank in board.Ranks())
            {
                ranks[id -1] = new string(boardRank.Select(ToChar).ToArray());
                id++;
            }
            Ranks = ranks;
        }


        private static readonly Dictionary<Chess.Board.PieceNames, char> OneCharPieceNames = new Dictionary<Chess.Board.PieceNames, char>
        {
            {Chess.Board.PieceNames.Blank, '.' },
            {Chess.Board.PieceNames.Rook, 'R' },
            {Chess.Board.PieceNames.Knight, 'N' },
            {Chess.Board.PieceNames.Bishop, 'B' },
            {Chess.Board.PieceNames.Queen, 'Q' },
            {Chess.Board.PieceNames.King, 'K' },
            {Chess.Board.PieceNames.Pawn, 'P' }
        };
    }
}