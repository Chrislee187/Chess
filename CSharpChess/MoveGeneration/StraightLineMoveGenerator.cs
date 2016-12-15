using System.Collections.Generic;
using System.Linq;
using CSharpChess.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    public class StraightLineMoveGenerator : MoveGeneratorBase
    {
        private readonly IEnumerable<Chess.Rules.MovementTransformation> _directions;

        protected StraightLineMoveGenerator(IEnumerable<Chess.Rules.MovementTransformation> directions)
        {
            _directions = directions;
        }

        public override IEnumerable<ChessMove> All(ChessBoard board, BoardLocation at)
        {
            var result = new List<ChessMove>();
            var directions = _directions;
            var piece = board[at].Piece;
            foreach (var direction in directions)
            {
                var locations = GetUntilNotEmpty(board, at, direction).ToList();

                result.AddRange(locations.Select(loc => new ChessMove(at, loc, MoveType.Move)));

                var last = locations.Any() ? locations.Last() : at;
                var next = direction.ApplyTo(last);

                if (next != null)
                {
                    var moveType = board[next].Piece.Colour == piece.Colour
                        ? MoveType.Cover
                        : board.CanTakeAt(next, piece.Colour)
                            ? MoveType.Take
                            : MoveType.Move;
                    result.Add(new ChessMove(at, next, moveType));
                }
            }

            return result;
        }

        private IEnumerable<BoardLocation> GetUntilNotEmpty(ChessBoard board, BoardLocation at, Chess.Rules.MovementTransformation movement)
        {
            var result = new List<BoardLocation>();

            var to = movement.ApplyTo(at);

            while (to != null && board.IsEmptyAt(to))
            {
                result.Add(to);
                to = movement.ApplyTo(to);
            }
            return result;
        }

    }
}