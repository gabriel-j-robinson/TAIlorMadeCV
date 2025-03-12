using System;

namespace TAILorMadeLib
{
    public class BadFileException : Exception
    {
        public BadFileException() { }

        public BadFileException(string message) : base(message) { }

        public BadFileException(string message, Exception inner) : base(message, inner) { }
    }

    public class UnsupportedFileException : Exception
    {
        public UnsupportedFileException() { }
        public UnsupportedFileException(string message) : base(message) { }

        public UnsupportedFileException(string message, Exception inner) : base(message, inner) { }
    }
}
