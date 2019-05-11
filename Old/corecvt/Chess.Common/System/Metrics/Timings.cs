using System.Collections.Generic;
using System.Linq;

namespace Chess.Common.System.Metrics
{
    public class Timings
    {
        public decimal Longest { get; }
        public decimal Shortest { get; }


        /// <summary>
        /// Average of values, excludes first value to filter out any warm-up time.
        /// </summary>
        public decimal Average { get; }

        public int Count { get; private set; }

        public static readonly Timings Empty = new Timings();

        private Timings()
        {
            
        }

        public Timings(IEnumerable<decimal> timings)
        {
            var times = timings as decimal[] ?? timings.ToArray();
            Shortest = times.Min();
            Longest = times.Max();
            Average = times.Count() > 1 
                ? times.OrderBy(t => t).Skip(1).Average()
                : times.First();

            Count = times.Count();
        }
    }
}