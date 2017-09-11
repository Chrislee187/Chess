namespace CSharpChess
{
    public enum GameState
    {
        BlackKingInCheck, WhiteKingInCheck, WaitingForMove, Stalemate,
        Unknown,
        CheckMateBlackWins,
        CheckMateWhiteWins,
        Draw
    }
}