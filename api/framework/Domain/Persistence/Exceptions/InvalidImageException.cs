using System.Runtime.Serialization;

namespace framework.Domain.Persistence;

[Serializable]
public class InvalidImageException : Exception
{
    public InvalidImageException()
    {
    }

    public InvalidImageException(string? message) : base(message)
    {
    }

    public InvalidImageException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public InvalidImageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}