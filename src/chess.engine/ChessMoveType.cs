namespace chess.engine
{
    public enum ChessMoveType
    {
        MoveOrTake, MoveOnly, TakeOnly, TakeEnPassant, Castle, Check, Checkmate,
        Promotion,
        Unknown,
        Taken,
        Cover, Invalid
    }
}