namespace framework.Domain.Persistence;

public class ImageVerifier : IImageVerifier
{
    private readonly Dictionary<byte[], Func<byte[], bool>> _imageFormatDecoders = new()
    {
        { new byte[] { 0x89, 0x50, 0x4E, 0x47 }, IsPng },
        { new byte[] { 0xFF, 0xD8 }, IsJpeg },
        { new byte[] { 0x42, 0x4D }, IsBmp },
        { new byte[] { 0x47, 0x49, 0x46 }, IsGif }
    };

    public bool IsImage(byte[] bytes)
    {
        return _imageFormatDecoders.Keys.Any(
            imageFormatDecoder => imageFormatDecoder.SequenceEqual(bytes.Take(imageFormatDecoder.Length)));
    }

    private static bool IsPng(byte[] bytes)
    {
        if (bytes.Length < 8)
            return false;
        return bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47 && bytes[4] == 0x0D &&
               bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A;
    }

    private static bool IsJpeg(byte[] bytes)
    {
        if (bytes.Length < 2)
            return false;
        return bytes[0] == 0xFF && bytes[1] == 0xD8;
    }

    private static bool IsBmp(byte[] bytes)
    {
        if (bytes.Length < 2)
            return false;
        return bytes[0] == 0x42 && bytes[1] == 0x4D;
    }

    private static bool IsGif(byte[] bytes)
    {
        if (bytes.Length < 3)
            return false;
        return bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46;
    }
}