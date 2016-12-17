using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CSharpChess.System.Metrics
{
    public static class Counters
    {
        public static IEnumerable<string> CounterKeys => InMemCounter.Keys;
        private static readonly ConcurrentDictionary<string, long> InMemCounter = new ConcurrentDictionary<string, long>();

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
}