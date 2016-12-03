using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.TheBoard
{
    public class BoardLocation
    {
        public int Rank { get; }
        public Chess.ChessFile File { get; }

        public BoardLocation(Chess.ChessFile file, int rank)
        {
            File = file;
            Rank = rank;
        }

        public static BoardLocation At(Chess.ChessFile file, int rank) => new BoardLocation(file, rank);

        public static BoardLocation At(int file, int rank) => new BoardLocation((Chess.ChessFile)file, rank);

        public static BoardLocation At(string at) => (BoardLocation) at;

        public static IEnumerable<BoardLocation> ListOf(params string[] locs)
        {
            var list = new List<BoardLocation>();

            foreach (var loc in locs)
            {
                list.Add((BoardLocation) loc);
            }

            return list;
        }


        #region Object overrides
        public override string ToString() => File.ToString().Substring(0, 1) + Rank;

        public static explicit operator BoardLocation(string s)
        {
            if (s.Length != 2) throw new ArgumentException($"Invalid BoardLocation {s}", nameof(s));

            int rank;
            Chess.ChessFile file;
            if (!Enum.TryParse(s[0].ToString().ToUpper(), out file)) throw new ArgumentException($"Invalid BoardLocation {s}", nameof(s));
            if (!int.TryParse(s[1].ToString(), out rank)) throw new ArgumentException($"Invalid BoardLocation {s}", nameof(s));

            return new BoardLocation(file, rank);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return obj.ToString() == ToString();
        }

        public override int GetHashCode() => ToString().GetHashCode();
        #endregion
    }

    public static class BoardLocationExtensions
    {
        public static BoardLocation ToBoardLocation(this string value)
        {
            return (BoardLocation) value;
        }
    }
}