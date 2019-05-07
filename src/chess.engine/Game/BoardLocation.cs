using System;
using chess.engine.Movement;

namespace chess.engine.Game
{
    // TODO: Refactor 'Colours' out of here
    public class BoardLocation : ICloneable
    {
        public int X { get; }
        public int Y { get; }

        private BoardLocation(int x, int y)
        {
            Guard.ArgumentException(() => OutOfBounds(x), $"Invalid x: {x}" );
            Guard.ArgumentException(() => OutOfBounds(y), $"Invalid y: {y}");
            Y = y;
            X = x;
        }

        public static BoardLocation At(int x, int y)
            => new BoardLocation(x, y);

        public static BoardLocation At(string at)
            => (BoardLocation)at;

        private bool OutOfBounds(int value) => value < 1 || value > 8;

        private BoardLocation SafeCreate(int x, int y)
        {
            if (OutOfBounds(y)) return null;
            if (OutOfBounds(x)) return null;

            return At(x, y);
        }

        public BoardLocation KnightVerticalMove(Colours colour, bool forward, bool right) => MoveForward(colour, forward ? 2 : -2)?.MoveRight(colour, right ? 1 : -1);
        public BoardLocation KnightHorizontalMove(Colours colour, bool forward, bool right) => MoveRight(colour, forward ? 2 : -2)?.MoveForward(colour, right ? 1 : -1);

        public BoardLocation MoveForward(Colours colour, int squares = 1) 
            => SafeCreate(X, Y + BoardMove.DirectionModifierFor(colour) * squares);

        public BoardLocation MoveBack(Colours colour, int squares = 1) 
            => SafeCreate(X, Y - BoardMove.DirectionModifierFor(colour) * squares);

        public BoardLocation MoveLeft(Colours colour, int squares = 1) 
            => SafeCreate(X - (BoardMove.DirectionModifierFor(colour) * squares), Y);

        public BoardLocation MoveRight(Colours colour, int squares = 1) 
            => SafeCreate(X + (BoardMove.DirectionModifierFor(colour) * squares), Y);

        #region Object overrides
        public override string ToString() => $"({X},{Y})";
        public object Clone() => At(X, Y);

        public static explicit operator BoardLocation(string s)
        {
            if (s.Length != 2) throw new ArgumentException($"Invalid BoardLocation {s}");

            // TODO: This is nice helper function but does make it dependent on ChessFile a Chess specific!!!
            if (!Enum.TryParse(s[0].ToString().ToUpper(), out ChessFile x)) throw new ArgumentException($"Invalid BoardLocation {s}");
            if (!int.TryParse(s[1].ToString(), out var y)) throw new ArgumentException($"Invalid BoardLocation {s}");

            return new BoardLocation((int) x, y);
        }

        protected bool Equals(BoardLocation other)
        {
            return X == other.X && Y == other.Y;
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
                return (X * 397) ^ Y;
            }
        }

        #endregion
    }
}