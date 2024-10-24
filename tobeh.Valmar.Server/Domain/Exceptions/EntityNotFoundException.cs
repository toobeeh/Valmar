namespace tobeh.Valmar.Server.Domain.Exceptions;

public class EntityNotFoundException(string message, bool fatal = true) : Exception(message)
{
    public bool Fatal => fatal;
}