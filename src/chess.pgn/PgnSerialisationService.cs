using System;
using System.Linq;
using chess.pgn.Json;
using chess.pgn.Parsing;
using Newtonsoft.Json;


namespace chess.pgn
{
    public class PgnSerialisationService
    {
        public string SerializeAllGames(string text, bool expandedFormat)
        {
            try
            {
                // TODO: Move all this in to a service
                var games = PgnReader.ReadAllGamesFromString(text);
                var settings = new JsonSerializerSettings();

                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                return expandedFormat
                    ? JsonConvert.SerializeObject(games, Formatting.Indented, settings)
                    : JsonConvert.SerializeObject(games.Select(p => new PgnJson(p)), Formatting.Indented, settings);
            }
            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }
        }
    }
}