namespace tobeh.Valmar.Server.Domain.Exceptions;

public class UserOperationException(string message, bool fatal = true) : Exception(message)
{
    public bool Fatal => fatal;
}