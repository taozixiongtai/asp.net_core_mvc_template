using System;

namespace mymvc_core.Services.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message)
            : base(message)
        {

        }
    }
}
