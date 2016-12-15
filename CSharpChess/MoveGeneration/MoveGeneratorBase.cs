using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    ///  <summary>
    ///  Move Generators are expected to generate a list of the available moves to a Piece on a supplied board.
    ///  
    ///  Even though the move generator has access to the board they are not expected to take any check states 
    ///  into account (i.e. King moves generated may put a king in check). 
    ///  
    ///  MoveGenerator results should only be made available through an instance of <see cref="ChessBoard"/> which 
    ///  will filter them against the current state of the board.
    ///  
    ///  <remarks>Although it would be nice to have the moves work out whether they involve check themselves, it makes the
    ///  generation of each pieces movelist more problematic as to generate to the move list of a piece, you need the move list of all other pieces
    ///  to ensure the move doesn't uncover check.
    /// 
    ///  Therefore the board will be resposible for managing this behaviour. See <see cref="ChessBoard.MovesFor"/>
    ///  </remarks>
    ///  </summary>
    public abstract class MoveGeneratorBase : IMoveGenerator
    {
        private readonly IDictionary<BoardLocation, IEnumerable<ChessMove>> _movesCache = new ConcurrentDictionary<BoardLocation, IEnumerable<ChessMove>>();
        private readonly IDictionary<BoardLocation, IEnumerable<ChessMove>> _coversCache = new ConcurrentDictionary<BoardLocation, IEnumerable<ChessMove>>();
        private readonly IDictionary<BoardLocation, IEnumerable<ChessMove>> _takesCache = new ConcurrentDictionary<BoardLocation, IEnumerable<ChessMove>>();

        public abstract IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at);

        protected IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            if (!_movesCache.ContainsKey(at))
            {
                _movesCache[at] = All(board, at).Where(m => m.MoveType.IsMove());
            }

            return _movesCache[at];
        }

        protected IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            if (!_takesCache.ContainsKey(at))
            {
                _takesCache[at] = All(board, at).Where(m => m.MoveType.IsTake());
            }
            return _takesCache[at];
        }

        protected IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at)
        {
            if (!_coversCache.ContainsKey(at))
            {
                _coversCache[at] = All(board, at).Where(m => m.MoveType.IsCover());
            }
            return _coversCache[at];
        }

    }
}