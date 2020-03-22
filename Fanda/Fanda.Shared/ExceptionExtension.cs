using System;

namespace Fanda.Shared
{
    public static class ExceptionExtension
    {
        public static string InnerMessage(this Exception ex)
        {
            Exception exception = ex;
            if (exception.InnerException != null)
            {
                exception = exception.InnerException;
                if (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                    if (exception.InnerException != null)
                    {
                        exception = exception.InnerException;
                    }
                }
            }

            return exception.Message;
        }
    }
}