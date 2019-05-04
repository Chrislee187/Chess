namespace chess.engine.Movement
{
    public enum ChessMoveType
    {
        // General moves, not chess specific
        MoveOrTake = 1, 
        MoveOnly = 2,
        TakeOnly = 3,
        UpdatePiece = 4,

        TakeEnPassant,
        KingMove,
        CastleKingSide,
        CastleQueenSide,
    }
}