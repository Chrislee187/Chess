using System.Linq;

namespace CSharpChess.System.Extensions
{
    public static class StringExtensions
    {
        public static string Repeat(this char s, int times) 
            => new string(s, times);
        public static string Repeat(this string s, int times)
            => Enumerable.Range(1, times).Aggregate("", (a, i) => a+s);
    }
}