using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
