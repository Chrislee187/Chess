using System.Collections.Generic;

namespace chess.engine.Pieces
{
    public class PawnMoveGenerator : IMoveGenerator
    {

        public IEnumerable<Move> MovesFrom(string location, Colours playerToMove)
            => MovesFrom(BoardLocation.At(location), playerToMove);

        public IEnumerable<Move> MovesFrom(BoardLocation location, Colours playerToMove)
        {
            var mod = Move.DirectionModifierFor(playerToMove);
            var endRank = Move.EndRankFor(playerToMove);
            var startRank = Pawn.StartRankFor(playerToMove);

            Guard.ArgumentException(() => location.Rank == endRank, $"{PieceName.Pawn} is invalid at {location}.");

            var moves = new List<Move>
            {
                Move.CreateMoveOnly(location, BoardLocation.At(location.File, location.Rank + mod))
            };

            if (location.Rank == startRank)
            {
                moves.Add(Move.CreateMoveOnly(location,
                    BoardLocation.At(location.File, location.Rank + (mod * 2))));
            }

            if (location.File != ChessFile.A)
            {
                moves.Add(Move.CreateTakeOnly(location,
                    BoardLocation.At(location.File - 1, location.Rank + mod)));
            }

            if (location.File != ChessFile.H)
            {
                moves.Add(Move.CreateTakeOnly(location,
                    BoardLocation.At(location.File + 1, location.Rank + mod)));
            }

            return moves;
        }

    }
}