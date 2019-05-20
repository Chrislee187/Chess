using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace board.engine.Movement
{
    /// <summary>
    /// Path is a sequence of Move's that require the previous moves to be valid before the next moves can be considered
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Path : IEnumerable<BoardMove>, ICloneable
    {
        // NOTE: string.Join make the debugger a little slow
        private string DebuggerDisplay
            => $"{string.Join(", ", _moves.Select(m => m.ToString()))}";

        private readonly List<BoardMove> _moves = new List<BoardMove>();

        public void Add(BoardMove move) => _moves.Add(move);
        public void AddRange(IEnumerable<BoardMove> moves) => _moves.AddRange(moves);

        public object Clone()
        {
            var clone = new Path();
            clone.AddRange(_moves.Select(m => m.Clone() as BoardMove));
            return clone;
        }

        public bool CanMoveTo(BoardLocation destination) 
            => this.Any(m => m.To.Equals(destination));

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

        public IEnumerator<BoardMove> GetEnumerator() => _moves.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

    }
}