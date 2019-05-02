using System.Collections.Generic;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement.Pawn
{
    public class PawnNormalAndStartingPathGenerator : IPathGenerator
    {
        public IEnumerable<Path> PathsFrom(BoardLocation location, Colours forPlayer)
        {
            var paths = new List<Path>();

            var oneSquareForward = location.MoveForward(forPlayer);

            if (oneSquareForward.Rank != ChessGame.EndRankFor(forPlayer))
            {
                var move = ChessMove.CreateMoveOnly(location, oneSquareForward);

                Path path = new Path();
                path.Add(move);
                if (location.Rank == Pieces.Pawn.StartRankFor(forPlayer))
                {
                    path.Add(ChessMove.CreateMoveOnly(location, location.MoveForward(forPlayer, 2)));
                }
                paths.Add(path);
            }
            else
            {
                foreach (var promotionPieces in new[] { ChessPieceName.Queen, ChessPieceName.Rook, ChessPieceName.Bishop, ChessPieceName.Knight })
                {
                    var move = ChessMove.CreatePawnPromotion(location, oneSquareForward, promotionPieces);
                    paths.Add(new Path { move });
                }
            }


            return paths;
        }

        public IEnumerable<Path> PathsFrom(string location, Colours forPlayer) => PathsFrom((BoardLocation) location, forPlayer);
    }
}