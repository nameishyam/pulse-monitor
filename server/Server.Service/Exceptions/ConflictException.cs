namespace Server.Service.Exceptions;

public class ConflictException(string message) : Exception(message);