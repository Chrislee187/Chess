using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpChess.TheBoard
{
    public class BoardLocation
    {
        public BoardLocation(Chess.ChessFile file, int rank)
        {
            File = file;
            Rank = rank;
        }

        public BoardLocation(string v)
        {
            var x = (BoardLocation) v;
            File = x.File;
            Rank = x.Rank;
        }

        public int Rank { get; }
        public Chess.ChessFile File { get; }

        public static implicit operator BoardLocation(string s)
        {
            if(s.Length != 2) throw new ArgumentException($"Invalid BoardLocation {s}", nameof(s));

            int rank;
            Chess.ChessFile file;
            if(!Enum.TryParse(s[0].ToString(), out file)) throw new ArgumentException($"Invalid BoardLocation {s}", nameof(s));
            if(!int.TryParse(s[1].ToString(), out rank)) throw new ArgumentException($"Invalid BoardLocation {s}", nameof(s));

            return new BoardLocation(file, rank);
        }

        public static BoardLocation At(Chess.ChessFile file, int rank)
        {
            return new BoardLocation(file, rank);
        }
        public static BoardLocation At(int file, int rank)
        {
            return new BoardLocation((Chess.ChessFile)file, rank);
        }

        public static BoardLocation At(string at)
        {
            return (BoardLocation) at;
        }

        public override string ToString() => File.ToString().Substring(0,1) + Rank;

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return obj.ToString() == ToString();
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public static IEnumerable<BoardLocation> Generate(params string[] locs)
        {
            var list = new List<BoardLocation>();

            foreach (var loc in locs)
            {
                list.Add(loc);
            }

            return list;
        }
    }
}