using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using chess.engine.Game;

namespace chess.engine.Movement
{
    /// <summary>
    /// Path is a sequence of Move's that require the previous move to be valid before the next move can be considered
    /// </summary>
    public class Path : IEnumerable<ChessMove>
    {
        private readonly List<ChessMove> _moves = new List<ChessMove>();


        public void Add(ChessMove move) => _moves.Add(move);

        #region Equality, Enumerator and Overrides

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

        public IEnumerator<ChessMove> GetEnumerator() => _moves.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

    }
    public class Paths : IEnumerable<Path>
    {
        private readonly List<Path> _paths = new List<Path>();


        public void Add(Path path) => _paths.Add(path);
        public void AddRange(IEnumerable<Path> paths) => _paths.AddRange(paths);

        #region Equality, Enumerator and Overrides

        protected bool Equals(Paths other) => _paths.All(other.Contains);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Paths)obj);
        }

        public override int GetHashCode() => _paths.GetHashCode();

        public override string ToString()
            => $"{string.Join(", ", _paths.Select(m => m.ToString()))}";

        public IEnumerator<Path> GetEnumerator() => _paths.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        public IEnumerable<ChessMove> FlattenMoves() => _paths.SelectMany(ps => ps);

        public bool ContainsMoveTo(BoardLocation location)
            => FlattenMoves().Any(m => m.To.Equals(location));
    }
}