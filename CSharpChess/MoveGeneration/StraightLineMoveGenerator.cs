using System.Collections.Generic;
using System.Linq;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CSharpChess.MoveGeneration
{
    public class StraightLineMoveGenerator : MoveGeneratorBase
    {
        private readonly IEnumerable<Chess.Board.LocationMover> _directions;

        protected StraightLineMoveGenerator(IEnumerable<Chess.Board.LocationMover> directions)
        {
            _directions = directions;
        }

        protected override IEnumerable<ChessMove> ValidMoves(ChessBoard board, BoardLocation at) 
            => GenerateAll(board, at).Moves();

        protected override IEnumerable<ChessMove> ValidCovers(ChessBoard board, BoardLocation at)
            => GenerateAll(board, at).Covers();

        protected override IEnumerable<ChessMove> ValidTakes(ChessBoard board, BoardLocation at)
            => GenerateAll(board, at).Takes();

        private IEnumerable<ChessMove> GenerateAll(ChessBoard board, BoardLocation at)
        {
            var result = new List<ChessMove>();
            var directions = _directions;
            var piece = board[at].Piece;
            foreach (var direction in directions)
            {
                var locations = Chess.Board.LocationMover.ApplyWhile(at, direction, board.IsEmptyAt).ToList();

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
    }
}