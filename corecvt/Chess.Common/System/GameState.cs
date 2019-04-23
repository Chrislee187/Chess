namespace Chess.Common.System
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