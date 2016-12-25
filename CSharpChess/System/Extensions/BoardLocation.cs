using System;
using System.Collections.Generic;

namespace CSharpChess.System.Extensions
{
    public class BoardLocation
    {
        public int Rank { get; }
        public Chess.Board.ChessFile File { get; }

        public BoardLocation(Chess.Board.ChessFile file, int rank)
        {
            File = file;
            Rank = rank;
        }

        public static BoardLocation At(Chess.Board.ChessFile file, int rank) 
            => new BoardLocation(file, rank);

        public static BoardLocation At(int file, int rank) 
            => new BoardLocation((Chess.Board.ChessFile)file, rank);

        public static BoardLocation At(string at) 
            => (BoardLocation) at;

        public static IEnumerable<BoardLocation> List(params string[] locs)
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
            Chess.Board.ChessFile file;
            if (!Enum.TryParse(s[0].ToString().ToUpper(), out file)) throw new ArgumentException($"Invalid BoardLocation {s}", nameof(s));
            if (!int.TryParse(s[1].ToString(), out rank))
                throw new ArgumentException($"Invalid BoardLocation {s}", nameof(s));

            return new BoardLocation(file, rank);
        }

        protected bool Equals(BoardLocation other)
        {
            return Rank == other.Rank && File == other.File;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BoardLocation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Rank*397) ^ (int) File;
            }
        }

        #endregion
    }
}