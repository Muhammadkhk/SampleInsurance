using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Framework.ApplicationService
{
    public class CommandResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public string ErrorMessage { get; private set; }

        public CommandResult(bool isSuccess, string message = null, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Message = message;
        }
    }
}
