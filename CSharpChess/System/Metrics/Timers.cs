using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using CSharpChess.Extensions;

namespace CSharpChess.System.Metrics
{
    public static class Timers
    {
        public static bool OutputTimesToConsole = false;
        public static bool DisableTiming = false;
        private static readonly ConcurrentDictionary<string, List<decimal>>  InMemTimings = new ConcurrentDictionary<string, List<decimal>>();
        public static IEnumerable<string> TimerKeys => InMemTimings.Keys;

        public static Timings GetTimingsFor(string counterKey)
        {
            if (!InMemTimings.ContainsKey(counterKey)) return Timings.Empty;

            var timings = InMemTimings[counterKey];
            if(timings.None()) return Timings.Empty;

            return new Timings(timings);
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