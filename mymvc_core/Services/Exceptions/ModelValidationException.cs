using System;

namespace mymvc_core.Services.Exceptions
{
    public abstract class ModelValidationException : ApplicationException
    {
        public ModelValidationException(string message)
            : base(message)
        {
        }
    }
}
