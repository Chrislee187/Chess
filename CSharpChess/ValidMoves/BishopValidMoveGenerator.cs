using System;
using System.Collections.Generic;
using System.Linq;
using CSharpChess.TheBoard;

namespace CSharpChess.ValidMoves
{
    public class BishopValidMoveGenerator : ValidMoveGeneratorBase
    {
        public BishopValidMoveGenerator() : base(Chess.PieceNames.Bishop)
        { }

        protected override IEnumerable<ChessMove> Moves(ChessBoard board, BoardLocation at)
        {
            var result = new List<ChessMove>();
            var directions = Chess.Rules.Bishops.DirectionTransformations;

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
            var directions = Chess.Rules.Bishops.DirectionTransformations;

            foreach (var direction in directions)
            {
                var lastEmpty = GetUntilNotEmpty(board, at, direction).Last();

                var to = ApplyDirection(lastEmpty, direction);

                if (to != null)
                {
                    if (board[to].Piece.Colour == Chess.ColourOfEnemy(board[at].Piece.Colour))
                    {
                        result.Add(new ChessMove(at, to, MoveType.Take));
                    }
                }
            }
            return result;
        }

        private BoardLocation ApplyDirection(BoardLocation from, Tuple<int, int> direction)
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
                to = ApplyDirection(to, direction);
            }

            if (board[to].Piece.Colour == Chess.ColourOfEnemy(board[at].Piece.Colour))
            {
                result.Add(to);
            }
            return result;
        }

    }
}