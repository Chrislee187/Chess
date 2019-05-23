using System;
using System.Collections.Generic;
using System.Linq;

namespace chess.webapi.Services
{
    public class PerfResult
    {
        public IEnumerable<TimeSpan> Times { get; }

        public PerfResult(string msg)
        {
            Error = msg;
        }

        public PerfResult(IEnumerable<TimeSpan> times)
        {
            Times = times;
        }

        public string Error { get; set; }

        public TimeSpan AverageParseGameTime
        {
            get
            {
                return new TimeSpan(Convert.ToInt64(Times.Average(ts => ts.Ticks)));
            }
        }
    }
}