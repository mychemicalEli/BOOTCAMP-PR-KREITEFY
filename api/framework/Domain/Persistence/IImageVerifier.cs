namespace framework.Domain.Persistence;

public interface IImageVerifier
{
    bool IsImage(byte[] bytes);
}