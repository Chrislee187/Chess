using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using chess.pgn.Json;
using chess.pgn.Parsing;
using Newtonsoft.Json;

namespace pgn2json
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader reader = Console.In;
            /*
             * If 1st param is not an option assume it's a file pattern otherwise read from stdin
             */
            if (args.Any())
            {
                Console.Error.WriteLine("No arguments yet, redirect standard input in using Get-Content/Type etc.");
                Environment.Exit(1);
            }

            var writer = Console.Out;

            var pgnReader = PgnReader.FromString(reader.ReadToEnd());

            var games = new List<PgnJson>();

            var readGame = pgnReader.ReadGame();
            while (readGame != null)
            {
                games.Add(new PgnJson(readGame));
                readGame = pgnReader.ReadGame();
            }
            var pgnJson = JsonConvert.SerializeObject(games, Formatting.Indented);
            writer.WriteLine(pgnJson);
        }
    }
}
