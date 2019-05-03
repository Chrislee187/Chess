using System.Collections.Generic;
using chess.engine.Entities;
using chess.engine.Movement;

namespace chess.engine.Game
{
    public class ActiveBoardPiece
    {
        public ChessPieceEntity Entity { get; }
        public Paths Paths { get; }
        public ActiveBoardPiece(ChessPieceEntity entityAt, Paths paths)
        {
            Entity = entityAt;
            Paths = paths;
        }
    }
}