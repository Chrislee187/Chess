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
#if DEBUG
        // NOTE: string.Join makes execution in DEBUG builds mode slower, event though the debugger display is not directly being used
        private string DebuggerDisplay=> $"{string.Join(", ", _moves.Select(m => m.ToString()))}";
#endif
        private readonly List<BoardMove> _moves;

        public void Add(BoardMove move) => _moves.Add(move);

        public Path() => _moves = new List<BoardMove>();

        private Path(IEnumerable<BoardMove> moves) => _moves = new List<BoardMove>(moves);

        public object Clone() => new Path(_moves.Select(m => m.Clone() as BoardMove));

        public bool CanMoveTo(BoardLocation destination) => this.Any(m => m.To.Equals(destination));

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


#if DEBUG
        public override string ToString()
        {
            return DebuggerDisplay;
        }
#endif

        #endregion
    }
}