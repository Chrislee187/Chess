using System.Collections.Generic;

namespace chess.engine.Pieces
{
    public class Pawn
    {
        public static int StartRankFor(Colours player)
            => player == Colours.White ? 2 : 7;

        public static IEnumerable<Move> MovesFrom(string location, Colours colour) => MovesFrom(BoardLocation.At(location), colour);

        public static IEnumerable<Move> MovesFrom(BoardLocation location, Colours playerToMove)
        {
            var mod = Move.DirectionModifierFor(playerToMove);
            var endRank = Move.EndRankFor(playerToMove);
            var startRank = StartRankFor(playerToMove);

            Guard.ArgumentException(() => location.Rank == endRank, $"{PieceName.Pawn} is invalid at {location}.");

            var moves = new List<Move>();
            moves.Add(Move.CreateMoveOnly(location, 
                BoardLocation.At(location.File, location.Rank + mod)));

            if (location.Rank == startRank)
            {
                moves.Add(Move.CreateMoveOnly(location,
                    BoardLocation.At(location.File, location.Rank + (mod *2))));
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