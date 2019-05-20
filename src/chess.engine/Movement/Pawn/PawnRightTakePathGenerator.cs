using System.Collections.Generic;
using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement.Pawn
{
    public class PawnRightTakePathGenerator : IPathGenerator
    {
        public Paths PathsFrom(BoardLocation location, int forPlayer)
        {
            var playerIdx = (Colours) forPlayer;
            Guard.ArgumentException(
                () => location.Y == (forPlayer == 0 ? 8 : 1),
                $"{ChessPieceName.Pawn} is invalid at {location}.");

            var paths = new Paths();

            var takeTypes = new List<int> {(int) DefaultActions.TakeOnly};

            if (location.Y == Pieces.Pawn.EnPassantRankFor(playerIdx))
            {
                takeTypes.Add((int) ChessMoveTypes.TakeEnPassant);
            }


            var takeLocation = location.MoveForward(playerIdx).MoveRight(playerIdx);
            if (takeLocation == null) return paths;

            if (takeLocation.Y != ChessGame.EndRankFor(playerIdx))
            {
                foreach (var takeType in takeTypes)
                {
                    var move = BoardMove.Create(location, takeLocation, takeType);
                    paths.Add(new Path { move });
                }
            }
            else
            {
                foreach (var promotionPieces in new[] { ChessPieceName.Queen, ChessPieceName.Rook, ChessPieceName.Bishop, ChessPieceName.Knight })
                {
                    var move = new BoardMove(location, takeLocation, (int) DefaultActions.UpdatePiece, new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData
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