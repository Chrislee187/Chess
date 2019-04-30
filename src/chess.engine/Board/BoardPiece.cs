using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Board
{
    public class BoardPiece
    {
        public BoardPiece(Colours colour, ChessPieceName name)
        {
            Colour = colour;
            Name = name;
        }

        public ChessPieceName Name{ get; }
        public Colours Colour{ get; }

        public override string ToString() => $"{Colour} {Name}";
    }
}