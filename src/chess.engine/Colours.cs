using System.Text;

namespace chess.engine
{
    public enum Colours { White, Black }

    public static class ColoursExtensions
    {
        public static T ConvertTo<T>(this Colours colour, T white, T black) =>
            colour == Colours.White ? white : black;
    }
}