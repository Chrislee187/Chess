namespace chess.engine.Game
{
    public static class ChessGameExtensions
    {
        public static string ToTextBoard(this ChessGame game)
        {
            return new ChessBoardBuilder().FromChessGame(game).ToTextBoard();
        }
        public static string ToBuilderCode(this ChessGame game)
        {
            return $"var builder = new ChessBoardBuilder()\r\n" +
                   $"   .Board(\r\n\"" +
                   ToTextBoard(game) + 
                   "\r\n);";
        }

    }
}