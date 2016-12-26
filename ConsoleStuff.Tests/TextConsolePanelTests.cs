using NUnit.Framework;

namespace ConsoleStuff.Tests
{
    public class TextConsolePanelTests
    {
        [Test]
        public void defaults_to_width_and_height_of_single_line_text()
        {
            const string text = "A Text Panel";
            var panel = new TextConsolePanel(text);

            Assert.That(panel.Height, Is.EqualTo(1));
            Assert.That(panel.Width, Is.EqualTo(text.Length));
        }

        [Test]
        public void defaults_to_width_and_height_of_multiline_text()
        {
            var longestItem = "With Multiple";
            string text = "A Text Panel\n" + longestItem + "\nLines";
            var panel = new TextConsolePanel(text);

            Assert.That(panel.Height, Is.EqualTo(3));
            Assert.That(panel.Width, Is.EqualTo(longestItem.Length));
        }
    }
}