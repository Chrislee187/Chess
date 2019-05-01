using System.Collections.Generic;

namespace spiker
{
    public class ConsoleWindowBuffer
    {
        public ConsoleWindowBuffer(int width, int height)
        {

        }

        public void AddWindow(ConsoleWindowBuffer window, int x, int y)
        {

        }

        public void Add(StringContent content, int x, int y)
        {
            // replace '\n' with move cursore to (x, y++)
        }
    }

    public class StringContent
    {
        public StringContent(string[] content)
        {
            // Auto set width and height from string
        }
        public StringContent(string[] content, int width = 0, int height = 0)
        {
            // Auto set width and height from string
        }
        public StringContent(string content) : this(content.Split('\n'))
        {
        }
    }

    public class BoxedContent
    {
        public BoxedContent(StringContent content, BoxConfig config)
        {

        }
        public class BoxConfig
        {
        }
    }

    public class TableContent<T>
    {
        public TableContent(IEnumerable<T> data, TableConfig<T> config)
        {

        }
        public class TableConfig<T>
        {
        }
    }


    public class ListBoxContent
    {

    }



}