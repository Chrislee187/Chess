using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement.Pawn
{
    public class PawnLeftTakePathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, int forPlayer)
        {
            var paths = new Paths();

            var playerIdx = (Colours) forPlayer;
            var takeType = location.Y == Pieces.Pawn.EnPassantRankFor(playerIdx)
                ? (int)ChessMoveTypes.TakeEnPassant
                : (int)DefaultActions.TakeOnly;

            var takeLocation = location.MoveForward(playerIdx).MoveLeft(playerIdx);

            if (takeLocation == null) return paths;
            if (takeLocation.Y != ChessGame.EndRankFor(playerIdx))
            {
                var move = BoardMove.Create(location, takeLocation, takeType);

                Path path = new Path();
                path.Add(move);
                paths.Add(path);
            }
            else
            {
                foreach (var promotionPieces in new[] { ChessPieceName.Queen, ChessPieceName.Rook, ChessPieceName.Bishop, ChessPieceName.Knight })
                {
                    var move = new BoardMove(location, takeLocation, (int) DefaultActions.UpdatePiece,
                        new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData
                        {
                            Owner = playerIdx,
                            PieceName = promotionPieces
                        });
                    paths.Add(new Path { move });
                }
            }

            return paths;
        }
    }
}