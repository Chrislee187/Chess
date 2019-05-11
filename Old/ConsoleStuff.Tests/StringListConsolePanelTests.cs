using ConsoleStuff.Panels;
using NUnit.Framework;

namespace ConsoleStuff.Tests
{
    public class StringListConsolePanelTests
    {
        private readonly string[] _items = { "item1", "seconditem" };

        [Test]
        public void panel_width_is_equal_to_longest_item()
        {
            var consoleMenu = new StringListConsolePanel(_items);

            Assert.That(consoleMenu.Width, Is.EqualTo(10));
        }

        [Test]
        public void panel_height_is_equal_to_number_of_items()
        {
            var consoleMenu = new StringListConsolePanel(_items);

            Assert.That(consoleMenu.Height, Is.EqualTo(2));
        }
    }
}