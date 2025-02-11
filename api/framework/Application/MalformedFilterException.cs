using System.Runtime.Serialization;

namespace framework.Application;
[Serializable]
public class MalformedFilterException : Exception
{
    public MalformedFilterException()
    {
    }

    public MalformedFilterException(string? message) : base(message)
    {
    }

    public MalformedFilterException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public MalformedFilterException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}