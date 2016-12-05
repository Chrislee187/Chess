using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class StraightLineValidMoveGenerator : ValidMoveGeneratorBase
    {
        private readonly IEnumerable<Tuple<int, int>> _directions;

        public StraightLineValidMoveGenerator(IEnumerable<Tuple<int, int>> directions, Chess.PieceNames name) : base(name)
        {
            _directions = directions;
        }

        protected override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var result = new List<ChessMove>();
            var directions = _directions;

            foreach (var direction in directions)
            {
                result.AddRange(
                    GetUntilNotEmpty(board, at, direction)
                        .Select(loc => new ChessMove(at, loc, MoveType.Move))
                );
            }

            return result;
        }

        protected override IEnumerable<ChessMove> Takes(ChessBoard board, BoardLocation at)
        {
            var result = new List<ChessMove>();
            var directions = _directions;

            foreach (var direction in directions)
            {
                var lastEmpty = GetUntilNotEmpty(board, at, direction)?.LastOrDefault();

                if (lastEmpty != null)
                {
                    var to = ApplyDirection(lastEmpty, direction);

                    if (to != null)
                    {
                        if (board[to].Piece.Colour == Chess.ColourOfEnemy(board[at].Piece.Colour))
                        {
                            result.Add(new ChessMove(at, to, MoveType.Take));
                        }
                    }
                }
            }
            return result;
        }

        protected override IEnumerable<BoardLocation> Threats(ChessBoard board, BoardLocation at)
        {
            throw new NotImplementedException();
        }

        public static BoardLocation ApplyDirection(BoardLocation from, Tuple<int, int> direction)
        {
            var file = (int)from.File + direction.Item1;
            var rank = from.Rank + direction.Item2;
            return !Chess.Board.IsValidLocation(file, rank)
                ? null
                : new BoardLocation((Chess.ChessFile)file, rank);
        }

        private IEnumerable<BoardLocation> GetUntilNotEmpty(ChessBoard board, BoardLocation at, Tuple<int, int> direction)
        {
            var result = new List<BoardLocation>();

            var to = ApplyDirection(at, direction);

            while (to != null && board.IsEmptyAt(to))
            {
                result.Add(to);
                to = ApplyDirection(to, direction);
            }
            return result;
        }

    }
}