using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CSharpChess.System.Metrics
{
    public static class Counters
    {
        private static readonly ConcurrentDictionary<string, long> InMemCounter = new ConcurrentDictionary<string, long>();

        public static IEnumerable<string> CounterKeys 
            => InMemCounter.Keys;

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
    }

    public static class Logger
    {
        // TODO: Wire this up to NLog/log4net or something
        private static readonly IDictionary<string, Action<string>> Loggers;

        static Logger()
        {
            Loggers = new Dictionary<string, Action<string>>()
            {{ "console", Console.WriteLine}}
            ;
        }

        public static void Log(string message)
        {
            foreach (var loggersValue in Loggers.Values)
            {
                loggersValue(message);
            }
        }
    }
}