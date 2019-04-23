namespace chess.engine
{
    public enum MoveType
    {
        MoveOrTake, MoveOnly, TakeOnly, TakeEnPassant, Castle, Check, Checkmate,
        Promotion,
        Unknown,
        Taken,
        Cover, Invalid
    }
}