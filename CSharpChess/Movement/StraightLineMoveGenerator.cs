using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpChess.Extensions;
using CSharpChess.System;

namespace CSharpChess.Movement
{
    public class StraightLineMoveGenerator : MoveGeneratorBase
    {
        private readonly IEnumerable<LocationFactory> _directions;

        protected StraightLineMoveGenerator(IEnumerable<LocationFactory> directions)
        {
            _directions = directions;
        }

        protected override IEnumerable<Move> ValidMoves(CSharpChess.Board board, BoardLocation at) 
            => GenerateAll(board, at).Moves();

        protected override IEnumerable<Move> ValidCovers(CSharpChess.Board board, BoardLocation at)
            => GenerateAll(board, at).Covers();

        protected override IEnumerable<Move> ValidTakes(CSharpChess.Board board, BoardLocation at)
            => GenerateAll(board, at).Takes();


        private readonly IDictionary<BoardPiece[,], IEnumerable<Move>> _allCache = new ConcurrentDictionary<BoardPiece[,], IEnumerable<Move>>();
        private IEnumerable<Move> GenerateAll(CSharpChess.Board board, BoardLocation at)
        {
            var key = board.BoardPieces;
            if (_allCache.ContainsKey(key)) return _allCache[key];

            var result = new List<Move>();
            var directions = _directions;
            var piece = board[at].Piece;

            foreach (var direction in directions)
            {
                var locations = LocationFactory.ApplyWhile(at, direction, board.IsEmptyAt).ToList();

                result.AddRange(locations.Select(loc => new Move(at, loc, MoveType.Move)));

                var last = locations.Any() ? locations.Last() : at;
                var next = direction.ApplyTo(last);

                if (next != null)
                {
                    var moveType = board[next].Piece.Colour == piece.Colour
                        ? MoveType.Cover
                        : board.CanTakeAt(next, piece.Colour)
                            ? MoveType.Take
                            : MoveType.Move;
                    result.Add(new Move(at, next, moveType));
                }
            }

            _allCache.Add(key, result);
            return result;
        }
    }
}