using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpChess.Helpers;
using CSharpChess.UnitTests.Helpers;

namespace ConsoleSpikes
{
    class Program
    {
        private IDictionary<string, Action> _menuItems = new ConcurrentDictionary<string, Action>();
        static void Main(string[] args)
        {
            var x = args.Length > 0 
                ? args 
                : new[] { "ColourBoardTest" };

            new Program().Run(x);
        }

        private void Run(string[] args)
        {
            foreach (var methodInfo in typeof(Program).GetMethods().Where(mi => mi.Name.StartsWith("MENU_")))
            {
                _menuItems.Add(methodInfo.Name.Substring(5).ToUpper(),() => methodInfo.Invoke(this, new object[] {}));
            }

            foreach (var s in args)
            {
                Console.WriteLine($"Executing {s}");
                _menuItems[s.ToUpper()]();
                Console.WriteLine("Press RETURN to end.");
                Console.ReadLine();
            }
        }

        public void MENU_ColourBoardTest()
        {
            var board = BoardBuilder.NewGame;

            SmallConsoleBoard.Write(board);

        }

        public void MENU_SimpleColourTest()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("In Red");
            Console.ResetColor();
        }
    }
}
