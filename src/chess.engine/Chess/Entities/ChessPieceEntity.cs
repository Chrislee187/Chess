using System.Collections.Generic;
using board.engine;
using board.engine.Movement;
using chess.engine.Game;

namespace chess.engine.Chess.Entities
{
    public abstract class ChessPieceEntity : IBoardEntity
    {
        public int EntityType { get; }
        public abstract IEnumerable<IPathGenerator> PathGenerators { get; }
        public string EntityName { get; }
        public int Owner { get; }


        public Colours Player { get; }
        public ChessPieceName Piece { get; protected set; }

        protected ChessPieceEntity(Colours player, ChessPieceName piece)
        {
            EntityType = (int) piece;
            EntityName = piece.ToString();
            Owner = (int) player;

            Player = player;
            Piece = piece;
        }

        public abstract object Clone();

        public override string ToString()
        {
            return $"{Player.ToString()} {EntityName}";
        }
    }
}