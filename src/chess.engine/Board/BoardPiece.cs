using chess.engine.Game;

namespace chess.engine.Board
{
    public class BoardPiece
    {
        private BoardPiece()
        {
            Colour = 0;
            Name = 0;
        }
        public BoardPiece(Colours colour, ChessPieceName name)
        {
            Colour = colour;
            Name = name;
        }

        public ChessPieceName Name{ get; set; }
        public Colours Colour{ get; set; }
        public static BoardPiece Empty => new BoardPiece();

        public override string ToString() => $"{Colour} {Name.ToString()}";
    }
}