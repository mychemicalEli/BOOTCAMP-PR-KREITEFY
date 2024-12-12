namespace api.Domain.Exceptions;

[Serializable]
public class InvalidEmailOrPasswordException : Exception
{
    public InvalidEmailOrPasswordException()
    {
    }

    public InvalidEmailOrPasswordException(string? message) : base(message)
    {
    }

    public InvalidEmailOrPasswordException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}