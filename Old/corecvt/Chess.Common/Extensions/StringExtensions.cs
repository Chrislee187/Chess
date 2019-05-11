using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chess.Common.Extensions
{
    public static class StringExtensions
    {
        private static readonly char[] EndOfLineChars = {'\r', '\n'};

        public static string Repeat(this char s, int times) 
            => new string(s, times);
        public static string Repeat(this string s, int times)
            => Enumerable.Range(1, times).Aggregate("", (a, i) => a+s);

        public static IEnumerable<string> ToLines(this string s, StringSplitOptions options = StringSplitOptions.None)
        {
            return s.Split(EndOfLineChars, options);
        }

        public static Stream ToStream(this string s)
        {
            MemoryStream stream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
            }
            return stream;
        }

        public static int ToInt(this string text)
        {
            int temp;
            int.TryParse(text, out temp);
            return temp;
        }

    }
}