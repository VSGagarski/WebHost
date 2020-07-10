using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class KisException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public KisException(ErrorCode errorCode)
            : this(errorCode, string.Empty)
        {
        }

        public KisException(ErrorCode errorCode, string message)
            : this(errorCode, message, null)
        {
        }

        public KisException(ErrorCode errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
