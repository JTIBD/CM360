using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Common
{
    public class AppError
    {
        public AppError(object Error, AppErrorType Type=AppErrorType.Default)
        {
            this.Error = Error;
            this.Type = Type;
        }
        public AppErrorType Type { get; set; }
        public object Error { get; set; }
    }
}
