using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement
{
    /// <summary>
    /// Path is a sequence of Move's that require the previous moves to be valid before the next moves can be considered
    /// </summary>
    public class Path : IEnumerable<BoardMove>, ICloneable
    {
        private readonly List<BoardMove> _moves = new List<BoardMove>();


        public void Add(BoardMove move) => _moves.Add(move);
        public void AddRange(IEnumerable<BoardMove> moves) => _moves.AddRange(moves);

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

        public object Clone()
        {
            var clone = new Path();
            clone.AddRange(_moves.Select(m => m.Clone() as BoardMove));
            return clone;
        }

        public IEnumerator<BoardMove> GetEnumerator() => _moves.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

    }
    public class Paths : IEnumerable<Path>, ICloneable
    {
        private readonly List<Path> _paths = new List<Path>();


        public void Add(Path path) => _paths.Add(path);
        public void AddRange(IEnumerable<Path> paths) => _paths.AddRange(paths);

        public object Clone()
        {
            var clone = new Paths();
            clone.AddRange(_paths.Select(ps => ps.Clone() as Path));
            return clone;
        }



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

        public IEnumerable<BoardMove> FlattenMoves() => _paths.SelectMany(ps => ps);

        public bool ContainsMoveTo(BoardLocation location)
            => FlattenMoves().Any(m => m.To.Equals(location));

        public BoardMove FindValidMove(BoardLocation from, BoardLocation destination, object promotionPiece = null)
        {
            return FlattenMoves()
                .SingleOrDefault(mv => mv.From.Equals(from)
                                       && mv.To.Equals(destination)
                                       && (promotionPiece == null
                                           || mv.UpdateEntityType.Equals(promotionPiece))
                );
        }

    }
}