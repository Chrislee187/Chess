using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using chess.engine.Board;

namespace chess.engine.Game
{
    public class ChessGame
    {
        private ChessBoardEngine _engine = new ChessBoardEngine().InitBoard();
        public Colours CurrentPlayer { get; private set; } = Colours.White;
        public bool InProgress = true;


        public BoardPiece[,] Board  => _engine.Board;

        public void Move(string input)
        {
            // Find move in valid moves list to get movetype
            var from = BoardLocation.At(input.Substring(0,2));
            var to = BoardLocation.At(input.Substring(2, 2));
            var validMove = _engine.PieceAt(@from).Paths.SelectMany(p => p).SingleOrDefault(p1 => p1.To.Equals(to));

            Guard.NotNull(validMove, $"{validMove} is invalid invalid!");

            // execute board action for move type
            _engine.Move(validMove);

            CurrentPlayer = CurrentPlayer == Colours.White ? Colours.Black : Colours.White;
        }
    }
}