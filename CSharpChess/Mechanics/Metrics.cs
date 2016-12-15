using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpChess.Mechanics
{
    public class Metrics
    {
        public static class Counters
        {
            public static class Board
            {
                public const string Created = "board.created.count";
            }
        }

        public static class Timers
        {
            public static class Board
            {
                public const string New = "board-creation.new-board.μs"; 
                public const string Empty = "board-creation.empty-board.μs";
                public const string Custom = "board-creation.custom-board.μs";
            }
        }
    }
}
