namespace chess.engine.Game
{
    // NOTE: King must be last to ensure it moves are checked after the others
    public enum ChessPieceName { Pawn = 1, Rook, Bishop, Knight, Queen, King = int.MaxValue}
}