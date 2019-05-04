namespace chess.engine.Movement
{
    public enum MoveType
    {
        // General moves, not chess specific
        MoveOrTake = 1, 
        MoveOnly = 2,
        TakeOnly = 3,
        UpdatePiece = 4,

        // ChessSpecific to be refactored in to something more generic
        TakeEnPassant,
        KingMove,
        CastleKingSide,
        CastleQueenSide,
    }
}