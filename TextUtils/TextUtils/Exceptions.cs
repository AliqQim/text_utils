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
    public class SnoskaNotFoundException : Fb2ToReadException
    {
        public SnoskaNotFoundException(string msg, Exception inner)
            : base(msg, inner)
        { }
    }

    public class UnusedSnoskaFoundException : Fb2ToReadException
    {
        public UnusedSnoskaFoundException(string msg)
            : base(msg)
        { }
    }
}
