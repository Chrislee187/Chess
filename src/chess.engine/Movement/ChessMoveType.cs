namespace chess.engine.Movement
{
    public enum ChessMoveType
    {
        MoveOrTake,
        MoveOnly,
        TakeOnly,
        TakeEnPassant,
        KingMove,
        Check,
        Castle,
        Checkmate,
        Promotion,
        Unknown,
        Taken,
        Cover,
        Invalid,
        CastleKingSide,
        CastleQueenSide
    }
}