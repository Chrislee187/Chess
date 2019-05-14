using board.engine;
using chess.engine.Game;

namespace chess.engine.Chess.Movement.ChessPieces.King
{
    public static class ChessBoardLocationExtensions
    {
        public static BoardLocation KnightVerticalMove(this BoardLocation location, Colours colour, bool forward, bool right) 
            => location.MoveForward(colour, forward ? 2 : -2)?.MoveRight(colour, right ? 1 : -1);
        public static BoardLocation KnightHorizontalMove(this BoardLocation location, Colours colour, bool forward, bool right) 
            => location.MoveRight(colour, forward ? 2 : -2)?.MoveForward(colour, right ? 1 : -1);

        public static BoardLocation MoveForward(this BoardLocation location, Colours colour, int squares = 1)
            => SafeCreate(location.X, location.Y + ChessGame.DirectionModifierFor(colour) * squares);

        public static BoardLocation MoveBack(this BoardLocation location, Colours colour, int squares = 1)
            => SafeCreate(location.X, location.Y - ChessGame.DirectionModifierFor(colour) * squares);

        public static BoardLocation MoveLeft(this BoardLocation location, Colours colour, int squares = 1)
            => SafeCreate(location.X - (ChessGame.DirectionModifierFor(colour) * squares), location.Y);

        public static BoardLocation MoveRight(this BoardLocation location, Colours colour, int squares = 1)
            => SafeCreate(location.X + (ChessGame.DirectionModifierFor(colour) * squares), location.Y);


        private static BoardLocation SafeCreate(int x, int y)
        {
            if (ChessGame.OutOfBounds(y)) return null;
            if (ChessGame.OutOfBounds(x)) return null;

            return BoardLocation.At(x, y);
        }
    }
}