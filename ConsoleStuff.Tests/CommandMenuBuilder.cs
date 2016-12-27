using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleStuff.Tests.Commands;

namespace ConsoleStuff.Tests
{
    public class CommandMenuBuilder
    {
        private readonly IList<Command> _commands = new List<Command>();
        public CommandMenuBuilder WithItem(string verb, Action<string> action, string desc = "")
        {
            _commands.Add(new Command(verb, action, desc));

            return this;
        }

        public CommandMenu Build()
        {
            return new CommandMenu(_commands.ToArray());
        }
    }
}