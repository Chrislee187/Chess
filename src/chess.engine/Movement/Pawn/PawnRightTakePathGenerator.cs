using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement.Pawn
{
    public class PawnRightTakePathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            Guard.ArgumentException(
                () => location.Rank == ChessMove.EndRankFor(forPlayer),
                $"{ChessPieceName.Pawn} is invalid at {location}.");

            var paths = new Paths();

            var takeType = location.Rank == Pieces.Pawn.EnPassantRankFor(forPlayer)
                ? ChessMoveType.TakeEnPassant
                : ChessMoveType.TakeOnly;


            var takeLocation = location.MoveForward(forPlayer).MoveRight(forPlayer);
            if (takeLocation == null) return paths;

            if (takeLocation.Rank != ChessGame.EndRankFor(forPlayer))
            {
                var move = ChessMove.Create(location, takeLocation, takeType);
                paths.Add(new Path {move});
            }
            else
            {
                foreach (var promotionPieces in new[] { ChessPieceName.Queen, ChessPieceName.Rook, ChessPieceName.Bishop, ChessPieceName.Knight })
                {
                    var move = ChessMove.CreatePawnPromotion(location, takeLocation, promotionPieces);
                    paths.Add(new Path { move });
                }
            }

            return paths;
        }

        public Paths PathsFrom(string location, Colours forPlayer) =>
            PathsFrom(BoardLocation.At(location), forPlayer);
    }
}