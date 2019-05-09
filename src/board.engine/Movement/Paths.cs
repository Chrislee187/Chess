using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using board.engine.Board;

namespace board.engine.Movement
{
    public class Paths : IEnumerable<Path>, ICloneable
    {
        private readonly List<Path> _paths = new List<Path>();
        
        public void Add(Path path) => _paths.Add(path);

        public void AddRange(IEnumerable<Path> paths) => _paths.AddRange(paths);

        public IEnumerable<BoardMove> FlattenMoves() => _paths.SelectMany(ps => ps);

        public bool ContainsMoveTo(BoardLocation location)
            => FlattenMoves().Any(m => m.To.Equals(location));

        public BoardMove FindValidMove(BoardLocation from, BoardLocation destination, object promotionPiece = null)
        {
            return FlattenMoves()
                .SingleOrDefault(mv => mv.From.Equals(from)
                                       && mv.To.Equals(destination)
                                       && (promotionPiece == null
                                           || mv.ExtraData.Equals(promotionPiece))
                );
        }

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

    }
}