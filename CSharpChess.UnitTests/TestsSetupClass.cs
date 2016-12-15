using System;
using CSharpChess.Mechanics;
using NUnit.Framework;

namespace CSharpChess.UnitTests
{
    [SetUpFixture]
    public class TestsSetupClass
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Do login here.
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // NOTE: Does not appear in R# runner output, does work from nunit-console
            Console.WriteLine("Counters");
            foreach (var m in new [] { Metrics.Counters.Board.Created })
            {
                Console.WriteLine($"{m.PadRight(35)}: {Counter.GetCountFor(m)}");
            }

            Console.WriteLine("Timers");
            foreach (var m in new [] {Metrics.Timers.Board.New, Metrics.Timers.Board.Custom, Metrics.Timers.Board.Empty})
            {
                Console.WriteLine($"Average : {m.PadLeft(35)} : {Counter.GetAvgTimeFor(m):       0} μs");
            }

            // NOTE: Doesn't fail in R# runner
            Assert.That(Counter.GetAvgTimeFor(Metrics.Timers.Board.New), Is.LessThan(5000), "Average build time for a new board is to slow"); // 5 ms to build a new board
            Assert.That(Counter.GetAvgTimeFor(Metrics.Timers.Board.Custom), Is.LessThan(5000), "Average build time for a custom board is to slow"); // 5 ms to build a new board
        }
    }
}