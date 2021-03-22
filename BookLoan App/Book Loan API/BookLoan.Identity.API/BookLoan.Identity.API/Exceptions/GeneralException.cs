using System;
using System.Runtime.Serialization;

namespace BookLoan.Identity.API.Exceptions
{
    [System.Serializable]
    public class GeneralException : Exception
    {
        public GeneralException()
        { }

        public GeneralException(string message)
            : base(message)
        { }

        public GeneralException(string message, Exception inner)
            : base(message, inner)
        { }

        protected GeneralException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
