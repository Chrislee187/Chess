using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public abstract class ValidMoveGeneratorBase
    {
        public abstract IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at);

        public IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at) 
            => All(board, at).Where(m => m.MoveType.IsMove());

        public IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at) 
            => All(board, at).Where(m => m.MoveType.IsTake());

        public IEnumerable<ChessMove> Covers(ChessBoard board, BoardLocation at) 
            => All(board, at).Where(m => m.MoveType.IsCover());

        protected virtual IEnumerable<BoardLocation> Threats(ChessBoard board, BoardLocation at)
        {
            var threats = new List<BoardLocation>();
            threats.AddRange(Moves(board, at).Select(m => m.To));
            threats.AddRange(Takes(board, at).Select(m => m.To));
            threats.AddRange(Covers(board, at).Select(m => m.To));
            return threats;
        }

        public IEnumerable<BoardLocation> ValidThreats(ChessBoard board, BoardLocation at) 
            => Threats(board, at);


    }
}