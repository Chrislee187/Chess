using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSharpChess.Mechanics
{
    public static class Counter
    {
        public static bool OutputTimesToConsole = false;
        public static bool DisableTiming = false;
        private static readonly ConcurrentDictionary<string, long> InMemCounter = new ConcurrentDictionary<string, long>();
        private static readonly ConcurrentDictionary<string, List<decimal>>  InMemTimings = new ConcurrentDictionary<string, List<decimal>>();
        public static void Increment(string counterKey)
        {
            InMemCounter.AddOrUpdate(counterKey, 1, (s, l) => l+1);
        }

        public static long GetCountFor(string counterKey)
        {
            long result = 0;
            InMemCounter.TryGetValue(counterKey, out result);
            return result;
        }
            
        public static decimal GetAvgTimeFor(string counterKey)
        {
            if (!InMemTimings.ContainsKey(counterKey)) return 0m;

            var timings = InMemTimings[counterKey];
            return timings.Any() ? timings.Average() : 0m;
        }

        public static void Time(string timerKey, Action action, bool toConsole = false)
        {
            if (DisableTiming)
            {
                action();
                return;
            }
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            action();

            var end = stopWatch.Elapsed;

            var microSeconds = (decimal) (end.TotalMilliseconds * 1000);

            InMemTimings.AddOrUpdate(timerKey, 
                (k) => new List<decimal>() {microSeconds}, 
                (k, l) => {
                    l.Add(microSeconds);
                    return l;
                });

            if (OutputTimesToConsole || toConsole)
            {
                Console.WriteLine($"{timerKey.PadRight(35)} : {microSeconds:    0000} μs");
            }
        }
    }
}