using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Common.Extensions;

namespace Chess.Common.System
{
    public class OneCharBoard
    {
        public IEnumerable<string> Ranks { get; }

        public static Colours PieceColour(char p)
        {
            if(p =='.' || p ==' ') return Colours.None;

            return char.IsLower(p)
                ? Colours.Black
                : Colours.White;
        }

        public static PieceNames PieceName(char p)
        {
            if(p =='.' || p ==' ') return PieceNames.Blank;

            var valid = OneCharPieceNames.Where(c => char.ToUpper(p) == c.Value).ToList();
            if (valid.None())
                throw new ArgumentException($"Invalid One Char representations of a chess piece found '{p}'", nameof(p));

            var pieceNames = OneCharPieceNames.First(c => char.ToUpper(p) == c.Value).Key;

            return pieceNames;
        }

        public static char ToChar(ChessPiece chessPiece)
        {
            var c = OneCharPieceNames[chessPiece.Name].ToString();
            if (chessPiece.Colour == Colours.Black) c = c.ToLower();
            return c[0];
        }

        public static ChessPiece ToChessPiece(char c) => new ChessPiece(PieceColour(c), PieceName(c));

        public OneCharBoard(Board board)
        {
            var ranks = new string[8];
            var id = 1;
            foreach (var boardRank in board.Ranks())
            {
                ranks[id -1] = new string(boardRank.Select(p => ToChar(p.Piece)).ToArray());
                id++;
            }
            Ranks = ranks;
        }

        private static readonly Dictionary<PieceNames, char> OneCharPieceNames = new Dictionary<PieceNames, char>
        {
            {PieceNames.Blank, '.' },
            {PieceNames.Rook, 'R' },
            {PieceNames.Knight, 'N' },
            {PieceNames.Bishop, 'B' },
            {PieceNames.Queen, 'Q' },
            {PieceNames.King, 'K' },
            {PieceNames.Pawn, 'P' }
        };
    }
}