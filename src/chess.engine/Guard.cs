using System;

namespace chess.engine
{
    public static class Guard
    {
        public static void ArgumentException(Func<bool> check, string message)
        {
            if(check())
                throw new ArgumentException(message);
        }

        public static void NotNull(object obj)
        {
            if(obj == null) throw new NullReferenceException();
        }
    }
}