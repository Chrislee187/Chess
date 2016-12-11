using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public abstract class MoveGeneratorBase : IMoveGenerator
    {
        private readonly IDictionary<BoardLocation, IEnumerable<ChessMove>> _movesCache = new ConcurrentDictionary<BoardLocation, IEnumerable<ChessMove>>();
        private readonly IDictionary<BoardLocation, IEnumerable<ChessMove>> _coversCache = new ConcurrentDictionary<BoardLocation, IEnumerable<ChessMove>>();
        private readonly IDictionary<BoardLocation, IEnumerable<ChessMove>> _takesCache = new ConcurrentDictionary<BoardLocation, IEnumerable<ChessMove>>();

        public abstract IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at);

        public IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            if (!_movesCache.ContainsKey(at))
            {
                _movesCache[at] = All(board, at).Where(m => m.MoveType.IsMove());
            }
            return _movesCache[at];
        }

        public IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            if (!_takesCache.ContainsKey(at))
            {
                _takesCache[at] = All(board, at).Where(m => m.MoveType.IsTake());
            }
            return _takesCache[at];
        }

        public IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at)
        {
            if (!_coversCache.ContainsKey(at))
            {
                _coversCache[at] = All(board, at).Where(m => m.MoveType.IsCover());
            }
            return _coversCache[at];
        }

        public void RecalcAll()
        {
            _movesCache.Clear();
            _takesCache.Clear();
            _coversCache.Clear();
        }
    }
}