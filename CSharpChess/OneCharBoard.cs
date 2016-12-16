using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess
{
    public class OneCharBoard
    {
        public IEnumerable<string> Ranks { get; }

        public static Chess.Colours PieceColour(char p)
        {
            if(p =='.' || p ==' ') return Chess.Colours.None;

            return char.IsLower(p)
                ? Chess.Colours.Black
                : Chess.Colours.White;
        }

        public static Chess.PieceNames PieceName(char p)
        {
            if(p =='.' || p ==' ') return Chess.PieceNames.Blank;

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
            if (p.Piece.Colour == Chess.Colours.Black) c = c.ToLower();
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


        private static readonly Dictionary<Chess.PieceNames, char> OneCharPieceNames = new Dictionary<Chess.PieceNames, char>
        {
            {Chess.PieceNames.Blank, '.' },
            {Chess.PieceNames.Rook, 'R' },
            {Chess.PieceNames.Knight, 'N' },
            {Chess.PieceNames.Bishop, 'B' },
            {Chess.PieceNames.Queen, 'Q' },
            {Chess.PieceNames.King, 'K' },
            {Chess.PieceNames.Pawn, 'P' }
        };
    }
}