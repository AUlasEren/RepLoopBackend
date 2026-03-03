namespace RepLoopBackend.SharedKernel.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string errorCode, string message)
        : base(400, errorCode, message) { }
}
