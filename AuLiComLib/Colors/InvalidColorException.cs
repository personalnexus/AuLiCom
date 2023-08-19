using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Colors
{
    public class InvalidColorException : Exception
    {
        public InvalidColorException()
        {
        }

        public InvalidColorException(string? message) : base(message)
        {
        }

        public InvalidColorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidColorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
