using System;
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

        [TestCase("A Text Panel")]
        [TestCase("A Text Panel\nWith more\nthan one line")]
        public void defaults_to_truncate_for_text_that_doesnt_fit(string text)
        {
            var panel = new TextConsolePanel(text, 5);

            Assert.That(panel.Width, Is.EqualTo(5));
            Assert.That(panel.ToStrings()[0], Is.EqualTo("A Tex"));
        }

    }
}