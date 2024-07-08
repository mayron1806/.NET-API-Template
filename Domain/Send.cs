namespace Domain;

public class Send
{
    public Send(string? message, string? password, bool quickDownload, bool expiresOnDowload, IEnumerable<string>? destination)
    {
        Message = message;
        Password = password;
        QuickDownload = quickDownload;
        ExpiresOnDowload = expiresOnDowload;
        Destination = destination;
    }
    public string? Message { get; }
    public string? Password { get; }
    public int Downloads { get; }
    public bool QuickDownload { get; }
    public bool ExpiresOnDowload { get; }
    public IEnumerable<string>? Destination { get; }
}
