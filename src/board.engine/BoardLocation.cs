using System;

namespace board.engine
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

        private bool OutOfBounds(int value) => value < 1 || value > 8;



        #region Object overrides
        public override string ToString() => $"({X},{Y})";
        public object Clone() => At(X, Y);

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