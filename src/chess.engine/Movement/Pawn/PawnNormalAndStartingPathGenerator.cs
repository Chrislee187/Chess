using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement.Pawn
{
    public class PawnNormalAndStartingPathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new Paths();

            var oneSquareForward = location.MoveForward(forPlayer);
            if (oneSquareForward.Rank != ChessGame.EndRankFor(forPlayer))
            {
                var move = BoardMove.CreateMoveOnly(location, oneSquareForward);

                var path = new Path {move};
                if (location.Rank == Pieces.Pawn.StartRankFor(forPlayer))
                {
                    path.Add(BoardMove.CreateMoveOnly(location, location.MoveForward(forPlayer, 2)));
                }
                paths.Add(path);
            }
            else
            {
                foreach (var promotionPieces in new[] { ChessPieceName.Queen, ChessPieceName.Rook, ChessPieceName.Bishop, ChessPieceName.Knight })
                {
                    var move = BoardMove.CreatePawnPromotion(location, oneSquareForward, promotionPieces);
                    paths.Add(new Path { move });
                }
            }


            return paths;
        }

        public Paths PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation) location, forPlayer);
    }
}