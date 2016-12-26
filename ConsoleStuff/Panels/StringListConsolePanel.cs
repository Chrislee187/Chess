using System.Collections.Generic;
using System.Linq;

namespace ConsoleStuff.Panels
{
    public class StringListConsolePanel : ConsolePanel
    {
        private readonly IEnumerable<string> _strings;

        public StringListConsolePanel(IEnumerable<string> commandMenuItems) 
            : base(commandMenuItems.Max(i => i.Length), commandMenuItems.Count())
        {
            _strings = commandMenuItems;


        }
    }
}