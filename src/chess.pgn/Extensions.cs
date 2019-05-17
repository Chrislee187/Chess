using System.IO;

namespace chess.pgn
{
    public static class Extensions
    {
        public static Stream ToStream(this string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
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