namespace Server.Service.Exceptions;

public class NotFoundException(string message) : Exception(message);