namespace CSharpChess.System.Metrics
{
    public static class TimerIds
    {
        public static class Board
        {
            public const string New = "board.creation.new";
            public const string Empty = "board.creation.empty";
            public const string Custom = "board.creation.custom";
            public const string RebuildMoveList = "board.movelists.rebuild-all";
        }
    }
}