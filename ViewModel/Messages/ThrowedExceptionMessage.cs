using System;

namespace NetChat2.ViewModel.Messages
{
    public class ThrowedExceptionMessage : IMessage
    {
        public string ErrorMessage { get; private set; }
        public Exception Exception { get; private set; }

        public ThrowedExceptionMessage(Exception exception, string errorMessage)
        {
            Exception = exception;
            ErrorMessage = errorMessage;
        }
    }
}
