using System.Collections.Generic;
using System.Linq;

// ReSharper disable PossibleMultipleEnumeration

namespace Chess.Common.Tests.Panels
{
    public class StringListConsolePanel : ConsolePanel
    {
        public StringListConsolePanel(IEnumerable<string> commandMenuItems) 
            : base(commandMenuItems.Max(i => i.Length), commandMenuItems.Count())
        {
        }
    }
}