using System.Collections.Generic;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Chess.Entities;
using chess.engine.Exceptions;
using chess.engine.Game;

namespace chess.engine.Algebraic
{
    public class SanMoveFinder
    {
        private readonly IBoardState<ChessPieceEntity> _boardState;

        public SanMoveFinder(IBoardState<ChessPieceEntity> boardState)
        {
            _boardState = boardState;
        }

        public BoardMove Find(StandardAlgebraicNotation san, Colours forPlayer)
        {
            var destination = BoardLocation.At(san.ToFileX, san.ToRankY);

            if (san.HaveFrom)
            {
                var exact = FindExactMove(san, destination);

                if(exact == null) throw new MoveFinderException($"Move not found: {san.ToNotation()}");

                return exact;
            }

            var items = _boardState
                    .GetItems((int)forPlayer, (int)san.Piece)
                    .Where(i => i.Paths.ContainsMoveTo(destination))
                ;

            if (TryFindMove(items, destination, out var move)) return move;
            
            if (san.FromFileX.HasValue)
            {
                items = items.Where(i => i.Paths.FlattenMoves().Any(m => m.From.X == san.FromFileX.Value));
                if (TryFindMove(items, destination, out move)) return move;
            }

            if (san.FromRankY.HasValue)
            {
                items = items.Where(i => i.Paths.FlattenMoves().Any(m => m.From.Y == san.FromRankY.Value));
                if (TryFindMove(items, destination, out move)) return move;
            }


            throw new MoveFinderException("Couldn't disambiguate move");
        }

        private BoardMove FindExactMove(StandardAlgebraicNotation san, BoardLocation destination)
        {
            var from = BoardLocation.At(san.FromFileX.Value, san.FromRankY.Value);
            var item = _boardState.GetItem(@from);

            var mv = item.Paths.FindValidMove(@from, destination);

            if (mv == null)
            {
                throw new MoveFinderException($"Cannot find move matching '{san}'");
            }

            return mv;
        }

        private static bool TryFindMove(IEnumerable<LocatedItem<ChessPieceEntity>> items, BoardLocation destination, out BoardMove findMoveTo)
        {
            findMoveTo = null;
            var locatedItems = items as LocatedItem<ChessPieceEntity>[] ?? items.ToArray();
            if (!locatedItems.Any())
            {
                return true;
            }

            if (locatedItems.Count() == 1)
            {
                findMoveTo = FindMoveTo(locatedItems.Single(), destination);
                return true;
            }

            return false;
        }

        private static BoardMove FindMoveTo(LocatedItem<ChessPieceEntity> item, BoardLocation destination) 
            => item.Paths.FlattenMoves().SingleOrDefault(m => m.To.Equals(destination));
    }
}