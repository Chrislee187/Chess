using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess
{
    public class OneCharBoard
    {
        public static Chess.Colours PieceColour(char p)
        {
            return char.IsLower(p)
                ? Chess.Colours.Black
                : char.IsUpper(p) ? Chess.Colours.White : Chess.Colours.None;
        }

        public static Chess.PieceNames PieceName(char p)
        {
            var valid = _oneCharMap.Where(c => char.ToUpper(p) == c.Value).ToList();
            if (!valid.Any())
                throw new ArgumentException($"Invalid One Char representations of a chess piece found '{p}'", nameof(p));

            var pieceNames = _oneCharMap.First(c => char.ToUpper(p) == c.Value).Key;
            Console.WriteLine($"{pieceNames} == {p}");
            return pieceNames;
        }

        private static readonly Dictionary<Chess.PieceNames, char> _oneCharMap = new Dictionary<Chess.PieceNames, char>
        {
            {Chess.PieceNames.Blank, '.' },
            {Chess.PieceNames.Rook, 'R' },
            {Chess.PieceNames.Knight, 'N' },
            {Chess.PieceNames.Bishop, 'B' },
            {Chess.PieceNames.Queen, 'Q' },
            {Chess.PieceNames.King, 'K' },
            {Chess.PieceNames.Pawn, 'P' }
        };

        public static char ToOneChar(BoardPiece p)
        {
            var c = _oneCharMap[p.Piece.Name].ToString();
            if (p.Piece.Colour == Chess.Colours.Black) c = c.ToLower();
            return c[0];
        }

        public OneCharBoard(ChessBoard board)
        {
            var ranks = new string[8];


            foreach (var boardRank in board.Ranks.OrderBy(r => r.Rank))
            {
                ranks[boardRank.Rank-1] = boardRank.ToString();
            }
            Ranks = ranks;
        }

        public IEnumerable<string> Ranks { get;  }

    }
}