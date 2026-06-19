namespace Server.Domain.Dto.Options;

public class SupabaseOptions
{
    public string Url { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}