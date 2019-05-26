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

            if (args.Any())
            {
                /*
                 * TODO: need YACLAP for some CLI options
                 */
                Console.Error.WriteLine("No arguments yet, redirect standard input in using Get-Content/Type etc.");
                Environment.Exit(1);
            }

            var writer = Console.Out;

            var games = PgnReader.ReadAllGamesFromString(reader.ReadToEnd());

            bool expandMoves = false; // TODO: Need a CLI option for this and indentation
            var pgnJson = expandMoves
                ? JsonConvert.SerializeObject(games, Formatting.Indented)
                : JsonConvert.SerializeObject(games.Select(g => new PgnJson(g)), Formatting.Indented);
            writer.WriteLine(pgnJson);
        }
    }
}
