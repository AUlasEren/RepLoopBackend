namespace RepLoopBackend.SharedKernel.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string errorCode, string name, object key)
        : base(404, errorCode, $"{name} bulunamadı. (id: {key})") { }

    public NotFoundException(string errorCode, string message)
        : base(404, errorCode, message) { }
}
