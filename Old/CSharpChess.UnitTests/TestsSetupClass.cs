using System;
using System.Linq;
using CSharpChess.System;
using CSharpChess.System.Metrics;
using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests
{
    [SetUpFixture]
    public class TestsSetupClass
    {
        // ReSharper disable once InconsistentNaming
        private const int MAX_AvgNewBoardBuildtimeInMicroSeconds = 5000;
        // ReSharper disable once InconsistentNaming
        private const int MAX_AvgCustomBoardBuildtimeInMicroSeconds = 5000;

        private readonly Func<string, int, string> _titlePadder = (s, i) => s.PadRight(i);
        private readonly Func<string, int, string> _valuePadder = (s, i) => s.PadLeft(i);
        private const int TitleWidth = 35;
        private const int ValueWidth = 9;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var board = BoardBuilder.NewGame;
            Assert.That(board.GameState, Is.EqualTo(GameState.WaitingForMove));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // NOTE: Console output does not appear in R# runner output under this class's entry, 
            // it appears in the output of the last TestFixture run!
            OutputCounters();
            Console.WriteLine();
            OutputTimers();

            AssertBoardsAreCreatedWithinAReasonableTime();
        }

        private void OutputCounters()
        {
            Console.WriteLine("Counters");
            foreach (var m in Counters.CounterKeys.OrderBy(l => l))
            {
                Console.WriteLine($"{_titlePadder(m, TitleWidth)} : {_valuePadder(Counters.GetCountFor(m).ToString(), ValueWidth)}");
            }
        }

        private void OutputTimers()
        {
            Console.WriteLine("Timers");
            Console.WriteLine(BuildTimerTableLine());
            foreach (var k in Timers.TimerKeys.OrderBy(l => l))
            {
                Console.WriteLine(BuildTimerTableLine(k));
            }
        }

        private string BuildTimerTableLine(string timerId = "")
        {
            string title, min, max, avg;

            if (timerId == string.Empty)
            {
                title = "Timer Id";
                min = "min (μs)";
                max = "max (μs)";
                avg = "avg (μs)";
            }
            else
            {
                title = timerId;
                var timer = Timers.GetTimingsFor(timerId);
                min = ((long) timer.Shortest).ToString();
                max = ((long) timer.Longest).ToString();
                avg = ((long) timer.Average).ToString();
            }
            return $"{_titlePadder(title,TitleWidth)} : {_valuePadder(min,ValueWidth)} |{_valuePadder(avg,ValueWidth)} |{_valuePadder(max,ValueWidth)}";
        }

        /// <summary>
        /// Aimed as an early catch against doing something stupid that causes board creation to take massively longer.
        /// </summary>
        private static void AssertBoardsAreCreatedWithinAReasonableTime()
        {
            if(Timers.GetTimingsFor(TimerIds.Board.New).Count > 1)
                AssertTimerAverageIsWithinRequiredTime(TimerIds.Board.New, MAX_AvgNewBoardBuildtimeInMicroSeconds);

            if (Timers.GetTimingsFor(TimerIds.Board.Custom).Count > 1)
                AssertTimerAverageIsWithinRequiredTime(TimerIds.Board.Custom, MAX_AvgCustomBoardBuildtimeInMicroSeconds);
        }

        private static void AssertTimerAverageIsWithinRequiredTime(string timerId, long maxAverage)
        {
            if (Timers.TimerKeys.Contains(timerId))
            {
                var avgNewBoards = Timers.GetTimingsFor(timerId).Average;
                Assert.That(avgNewBoards, Is.LessThan(maxAverage),
                    $"Average time for {timerId} is greater than required minimum.");
            }
        }
    }
}