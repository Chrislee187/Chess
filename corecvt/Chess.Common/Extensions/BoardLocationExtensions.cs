namespace Chess.Common.Extensions
{
    public static class BoardLocationExtensions
    {
        public static BoardLocation ToBoardLocation(this string value)
        {
            return (BoardLocation) value;
        }
    }
}