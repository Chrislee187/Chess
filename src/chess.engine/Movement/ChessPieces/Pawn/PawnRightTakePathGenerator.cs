using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement.ChessPieces.Pawn
{
    public class PawnRightTakePathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            Guard.ArgumentException(
                () => location.Rank == BoardMove.EndRankFor(forPlayer),
                $"{ChessPieceName.Pawn} is invalid at {location}.");

            var paths = new Paths();

            var takeType = location.Rank == Pieces.Pawn.EnPassantRankFor(forPlayer)
                ? MoveType.TakeEnPassant
                : MoveType.TakeOnly;


            var takeLocation = location.MoveForward(forPlayer).MoveRight(forPlayer);
            if (takeLocation == null) return paths;

            if (takeLocation.Rank != ChessGame.EndRankFor(forPlayer))
            {
                var move = BoardMove.Create(location, takeLocation, takeType);
                paths.Add(new Path {move});
            }
            else
            {
                foreach (var promotionPieces in new[] { ChessPieceName.Queen, ChessPieceName.Rook, ChessPieceName.Bishop, ChessPieceName.Knight })
                {
                    var move = BoardMove.CreatePawnPromotion(location, takeLocation, promotionPieces);
                    paths.Add(new Path { move });
                }
            }

            return paths;
        }

        public Paths PathsFrom(string location, Colours forPlayer) =>
            PathsFrom(BoardLocation.At(location), forPlayer);
    }
}