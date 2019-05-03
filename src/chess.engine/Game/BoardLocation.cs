using System;
using chess.engine.Movement;

namespace chess.engine.Game
{
    public class BoardLocation : ICloneable
    {
        public int Rank { get; }
        public ChessFile File { get; }

        public BoardLocation(ChessFile file, int rank)
        {
            Guard.ArgumentException(() => OutOfBounds(rank), $"Invalid rank: {rank}" );
            Guard.ArgumentException(() => OutOfBounds((int) file), $"Invalid file: {file}");
            File = file;
            Rank = rank;
        }

        public static BoardLocation At(ChessFile file, int rank)
            => new BoardLocation(file, rank);

        public static BoardLocation At(int file, int rank)
            => At((ChessFile)file, rank);

        public static BoardLocation At(string at)
            => (BoardLocation)at;

        private bool OutOfBounds(int value) => value < 1 || value > 8;

        private BoardLocation SafeCreate(int file, int rank)
        {
            if (OutOfBounds(rank)) return null;
            if (OutOfBounds(file)) return null;

            return At(file, rank);
        }

        public BoardLocation KnightVerticalMove(Colours colour, bool forward, bool right) => MoveForward(colour, forward ? 2 : -2)?.MoveRight(colour, right ? 1 : -1);
        public BoardLocation KnightHorizontalMove(Colours colour, bool forward, bool right) => MoveRight(colour, forward ? 2 : -2)?.MoveForward(colour, right ? 1 : -1);

        public BoardLocation MoveForward(Colours colour, int squares = 1) 
            => SafeCreate((int)File, Rank + ChessMove.DirectionModifierFor(colour) * squares);

        public BoardLocation MoveBack(Colours colour, int squares = 1) 
            => SafeCreate((int)File, Rank - ChessMove.DirectionModifierFor(colour) * squares);

        public BoardLocation MoveLeft(Colours colour, int squares = 1) 
            => SafeCreate((int) File - (ChessMove.DirectionModifierFor(colour) * squares), Rank);

        public BoardLocation MoveRight(Colours colour, int squares = 1) 
            => SafeCreate((int) File + (ChessMove.DirectionModifierFor(colour) * squares), Rank);

        #region Object overrides
        public override string ToString() => File.ToString().Substring(0, 1) + Rank;
        public object Clone() => At(File, Rank);

        public static explicit operator BoardLocation(string s)
        {
            if (s.Length != 2) throw new ArgumentException($"Invalid BoardLocation {s}");

            int rank;
            ChessFile file;
            if (!Enum.TryParse(s[0].ToString().ToUpper(), out file)) throw new ArgumentException($"Invalid BoardLocation {s}");
            if (!int.TryParse(s[1].ToString(), out rank)) throw new ArgumentException($"Invalid BoardLocation {s}");

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
            return Equals((BoardLocation)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Rank * 397) ^ (int)File;
            }
        }

        #endregion
    }
}