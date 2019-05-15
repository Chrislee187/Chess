﻿using System;
using System.Runtime.Serialization;

namespace chess.engine.Exceptions
{
    [Serializable]
    public class MoveFinderException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MoveFinderException()
        {
        }

        public MoveFinderException(string message) : base(message)
        {
        }

        public MoveFinderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MoveFinderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}