namespace CSharpChess.Pgn
{
    public class PgnDate
    {
        public int? Year { get; }
        public int? Month { get; }
        public int? Day { get; }

        private PgnDate(int? year, int? month, int? day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public static PgnDate Parse(string value)
        {
            var bits = value.Split('.');
            int? year = null, month = null, day = null;

            if (bits.Length >= 1)
            {
                year = int.Parse(bits[0]);
            }

            if (bits.Length >= 2)
            {
                month = int.Parse(bits[1]);
            }

            if (bits.Length >= 3)
            {
                day = int.Parse(bits[2]);
            }

            return new PgnDate(year, month, day);
        }

        public override string ToString() => $"{Year:0000}.{Month:00}.{Day:00}";
    }
}