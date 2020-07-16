using System;
using System.Collections.Generic;
using System.Text;

namespace Fb2ToReadAloudText
{
    public class Fb2ToReadException : Exception
    {
        public Fb2ToReadException(string msg, Exception inner)
            : base(msg, inner)
        { }

        public Fb2ToReadException(string msg)
            : base(msg)
        { }
    }

    public class Fb2BodyNotFound: Fb2ToReadException
    {
        public Fb2BodyNotFound() :
            base("в FB2-файле не обнаружена секция Body") { }
    }
    public class FootnoteNotFoundException : Fb2ToReadException
    {
        public FootnoteNotFoundException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }

    public class UnusedFootnoteFoundException : Fb2ToReadException
    {
        public UnusedFootnoteFoundException(string msg)
            : base(msg)
        { }
    }
}
