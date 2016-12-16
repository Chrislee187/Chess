namespace CSharpChess.System.Metrics
{
    public static class TimerIds
    {
        public static class Board
        {
            public const string New = "board-creation.new.μs";
            public const string Empty = "board-creation.empty.μs";
            public const string Custom = "board-creation.custom.μs";
        }
    }
}