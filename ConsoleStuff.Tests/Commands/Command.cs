using System;

namespace ConsoleStuff.Tests.Commands
{
    public class Command
    {
        public string OneLiner { get; }
        public bool IsDefault { get; }
        public string Verb { get; }
        public Action<string> Action { get; }

        private Command(MenuItem menuItem, Action<string> command)
        {
            Verb = menuItem.Verb;
            Action = command;
        }

        public Command(string verb, Action<string> command, string oneLiner = "", bool isDefault = false) : this(new MenuItem(verb), command)
        {
            OneLiner = oneLiner;
            IsDefault = isDefault;
        }
    }
}