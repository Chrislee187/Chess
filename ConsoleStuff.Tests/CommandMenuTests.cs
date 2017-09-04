using System;
using ConsoleStuff.Tests.Commands;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement

namespace ConsoleStuff.Tests
{
    [TestFixture]
    public class CommandMenuTests
    {
        [Test]
        public void simple_verb_can_be_used()
        {
            var wasCalled = false;
            var command = new Command("simple", (mi) => wasCalled = true);

            var menu = new CommandMenu(command);

            var result = menu.Execute("simple");

            AssertCommandExecuted(result);
            AssertCommandContentsExecuted(wasCalled);
        }

        [Test]
        public void distinct_partial_can_be_used()
        {
            var wasCalled = false;
            var command = new Command("simple", (mi) => wasCalled = false);
            var command2 = new Command("reallysimple", (mi) => wasCalled = true);

            var menu = new CommandMenu(command, command2);

            menu.Execute("real");
            Assert.That(wasCalled, Is.True);
            menu.Execute("simp");
            Assert.That(wasCalled, Is.False);
        }

        [Test]
        public void verb_can_have_params()
        {
            const string expectedParamString = "param1 param2";

            var actualParamString = "";
            var command = new Command("with-params", (p) => actualParamString = p);
            var menu = new CommandMenu(command);

            var result = menu.Execute("with-params param1 param2");
            
            AssertCommandExecuted(result);
            Assert.That(actualParamString, Is.EqualTo(expectedParamString));
        }

        [Test]
        public void default_is_called_if_no_match_found()
        {
            bool aWasCalled = false;
            bool defaultWasCalled = false;

            var aCommand = new Command("a-command", (p) => { aWasCalled = true; });
            var anotherCommand = new Command("defaultcmd", (p) => { defaultWasCalled = true; }, isDefault: true);

            var menu = new CommandMenu(aCommand, anotherCommand);

            var result = menu.Execute("notfound param1 param2");

            AssertCommandExecuted(result);
            Assert.False(aWasCalled);
            Assert.True(defaultWasCalled);
        }

        [Test]
        public void cannot_set_more_than_one_default()
        {
            var aCommand = new Command("a-command", (p) => { }, isDefault: true);
            var anotherCommand = new Command("defaultcmd", (p) => { }, isDefault: true);

            Assert.Throws<ArgumentException>(() => new CommandMenu(aCommand, anotherCommand));
        }

        [Test]
        public void command_help_list_is_generated()
        {
            var aCommand = new Command("a-command", (p) => {  }, "A one line command description");
            var anotherCommand = new Command("defaultcmd", (p) => {  }, isDefault: true);

            var menu = new CommandMenu(aCommand, anotherCommand);

            Assert.That(menu.HelpText(), Is.Not.Empty);
            Console.WriteLine(menu.HelpText());
        }

        private static void AssertCommandContentsExecuted(bool wasCalled) 
            => Assert.True(wasCalled, "Command seemed to execute successfully but action contents were not executed!");

        private static void AssertCommandExecuted(bool result) 
            => Assert.True(result, "Command not found");
    }
}
