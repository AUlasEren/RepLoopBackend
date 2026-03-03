namespace RepLoopBackend.SharedKernel.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string errorCode, string message = "Bu işlem için yetkiniz yok.")
        : base(403, errorCode, message) { }
}
