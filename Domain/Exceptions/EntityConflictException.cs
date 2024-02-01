namespace Valmar.Domain.Exceptions;

public class EntityConflictException(string message) : Exception(message);