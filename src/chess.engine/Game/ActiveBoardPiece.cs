using System.Collections.Generic;
using chess.engine.Entities;
using chess.engine.Movement;

namespace chess.engine.Game
{
    public class ActiveBoardPiece
    {
        public ChessPieceEntity Entity { get; }
        public IEnumerable<Path> Paths { get; }
        public ActiveBoardPiece(ChessPieceEntity entityAt, IEnumerable<Path> paths)
        {
            Entity = entityAt;
            Paths = paths;
        }
    }
}