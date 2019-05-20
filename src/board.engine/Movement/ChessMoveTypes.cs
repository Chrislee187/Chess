namespace board.engine.Movement
{
    public enum ChessMoveTypes
    {
        // ChessSpecific to be refactored in to something more generic
        TakeEnPassant=1000,
        KingMove,
        CastleKingSide,
        CastleQueenSide,
    }
}