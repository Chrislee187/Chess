namespace board.engine.Movement
{
    public enum ChessMoveTypes
    {
        // ChessSpecific to be refactored in to something more generic
        TakeEnPassant=5,
        KingMove = 6,
        CastleKingSide = 7,
        CastleQueenSide = 8,
    }
}