namespace Valmar.Domain.Exceptions;

public class EntityAlreadyExistsException(string message) : Exception(message);