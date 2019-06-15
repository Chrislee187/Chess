using System.Linq;
using System.Text.Json;

namespace chess.blazor.Extensions
{
    public static class JsonDocumentExtensions
    {
        public static string GetString(this JsonElement element, string propertyName)
        {
            return element.EnumerateObject()
                .Single(jo => jo.Name.Equals("board"))
                .Value.GetString();

        }
    }
}