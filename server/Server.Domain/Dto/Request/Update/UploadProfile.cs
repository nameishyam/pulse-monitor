namespace Server.Domain.Dto.Request.Update;

public class UploadProfile
{
    public byte[] Bytes { get; init; } = null!;
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
}