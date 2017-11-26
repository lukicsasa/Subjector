using System;
using System.Collections.Generic;
using System.Text;

namespace Subjector.Common.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message)
        {

        }
    }
}
