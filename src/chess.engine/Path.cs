using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace chess.engine
{
    /// <summary>
    /// Path is a sequence of Move's that require the previous move to be valid before the next move can be considered
    /// </summary>
    public class Path : IEnumerable<Move>
    {
        private readonly List<Move> _moves = new List<Move>();

        public IEnumerator<Move> GetEnumerator() => _moves.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Move move) => _moves.Add(move);

        protected bool Equals(Path other) => _moves.All(other.Contains);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Path) obj);
        }

        public override int GetHashCode() => _moves.GetHashCode();

        public override string ToString() 
            => $"{string.Join(", ", _moves.Select(m=> m.ToString()))}";
    }
}