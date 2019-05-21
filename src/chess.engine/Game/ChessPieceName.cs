namespace chess.engine.Game
{
    // NOTE: King must be last to ensure it moves are checked after the others
    public enum ChessPieceName { Pawn = 1, Rook=2, Bishop=3, Knight=4, Queen=6, King = int.MaxValue}
}