using NUnit.Framework;

namespace chess.engine.tests
{
    public class AppContainerTests
    {
        [Test]
        public void Should_resolve_all_services()
        {
            var x = AppContainer.ServiceProvider;
            var count = 0;
            foreach (var service in AppContainer.ServiceCollection)
            {
                var ns = service.ServiceType.Namespace;
                if (ns.StartsWith("board.") || ns.StartsWith("chess."))
                {
                    var y = x.GetService(service.ServiceType);
                    count++;
                }
            }

            Assert.That(count, Is.GreaterThan(0), "No services resolved!");
        }
    }
}