using System;
using System.Collections.Generic;
using static CSharpChess.Chess;

namespace CSharpChess.System.Extensions
{
    public class BoardLocation
    {
        public int Rank { get; }
        public ChessFile File { get; }

        public BoardLocation(ChessFile file, int rank)
        {
            File = file;
            Rank = rank;
        }

        public static BoardLocation At(ChessFile file, int rank) 
            => new BoardLocation(file, rank);

        public static BoardLocation At(int file, int rank) 
            => new BoardLocation((ChessFile)file, rank);

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
            if (s.Length != 2) throw new ArgumentException($"Invalid BoardLocation {s}");

            int rank;
            ChessFile file;
            if (!Enum.TryParse(s[0].ToString().ToUpper(), out file)) throw new ArgumentException($"Invalid BoardLocation {s}");
            if (!int.TryParse(s[1].ToString(), out rank))            throw new ArgumentException($"Invalid BoardLocation {s}");

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