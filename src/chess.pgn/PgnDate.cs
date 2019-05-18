namespace chess.pgn
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

        public static PgnDate Parse(string pgnDateString)
        {
            var dateParts = pgnDateString.Split('.');
            int? year = null, month = null, day = null;

            if (dateParts.Length >= 1)
            {
                year = dateParts[0].ToInt();
            }

            if (dateParts.Length >= 2)
            {
                month = dateParts[1].ToInt();
            }

            if (dateParts.Length >= 3)
            {
                day = dateParts[2].ToInt();
            }

            return new PgnDate(year, month, day);
        }

        public override string ToString() => $"{Year:0000}.{Month:00}.{Day:00}";
    }
}