using System;
using System.Linq;
using chess.pgn.Json;
using Newtonsoft.Json;


namespace chess.pgn
{
    public interface IPgnSerialisationService
    {
        string SerializeAllGames(string text, bool expandedFormat);
    }

    public class PgnSerialisationService : IPgnSerialisationService
    {
        public string SerializeAllGames(string text, bool expandedFormat)
        {
            try
            {
                var games = PgnGame.ReadAllGamesFromString(text);
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