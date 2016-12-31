namespace ConsoleStuff.Panels
{
    public class FilledConsolePanel : ConsolePanel
    {
        public FilledConsolePanel(int width, int height, char fillChar, ConsoleCellColour fillColour = null)
            : base(width, height)
        {
            Fill(fillChar, fillColour);
        }
    }
}