namespace chess.engine.Game
{
    public static class ChessGameExtensions
    {
        public static string ToText(this ChessGame game)
        {
            return new ChessBoardBuilder().FromChessGame(game).ToString();
        }
    }
}