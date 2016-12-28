using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleStuff.Tests.Commands;

namespace ConsoleStuff.Tests
{
    public class CommandMenuBuilder
    {
        private readonly IList<Command> _commands = new List<Command>();
        public CommandMenuBuilder WithItem(string verb, Action<string> action, string desc = "", bool isDefault = false, bool visible = true)
        {
            _commands.Add(new Command(verb, action, desc, isDefault: isDefault, visible: visible));

            return this;
        }

        public CommandMenu Build()
        {
            return new CommandMenu(_commands.ToArray());
        }
    }
}