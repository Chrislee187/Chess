using System.Collections.Generic;
using chess.engine.Game;
using chess.engine.Movement;

namespace chess.engine.Entities
{
    public abstract class ChessPieceEntity
    {
        protected ChessPieceEntity(ChessPieceName piece, Colours owner)
        {
            EntityType = piece;
            Player = owner;
        }
        public ChessPieceName EntityType { get; }

        public Colours Player { get; }
        public abstract IEnumerable<IPathGenerator> PathGenerators { get; }
    }
}