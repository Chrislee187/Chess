namespace chess.engine.Game
{
    public static class ChessGameExtensions
    {
        public static string ToText(this ChessGame game)
        {
            return new ChessBoardBuilder().FromChessGame(game).ToString();
        }
        public static string ToBuilderCode(this ChessGame game)
        {
            return $"var builder = new ChessBoardBuilder()\r\n" +
                   $"   .Board(\r\n\"" +
                   ToText(game) + 
                   "\r\n);";
        }

    }
}