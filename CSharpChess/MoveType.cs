namespace CSharpChess
{
    public enum MoveType
    {
        Move, Take, TakeEnPassant, Castle, Check, Checkmate,
        Promotion,
        Unknown,
        Taken,
        Cover, Invalid
    }
}