using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }

    public class DailyTaskAlreadySubmittedException : Exception
    {
        public DailyTaskAlreadySubmittedException(string message) : base(message)
        {
        }
    }
    public class DefaultException : Exception
    {
        public DefaultException(string message) : base(message)
        {
        }
    }
}
