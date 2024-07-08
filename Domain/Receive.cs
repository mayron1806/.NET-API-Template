namespace Domain;

public class Receive
{
    public Receive(bool received, string? message, IEnumerable<string>? acceptedFiles, int maxSize, string? password)
    {
        Received = received;
        Message = message;
        AcceptedFiles = acceptedFiles;
        MaxSize = maxSize;
        Password = password;
    }
    public bool Received { get; }
    public string? Message { get; }
    public IEnumerable<string>? AcceptedFiles { get; }
    public int MaxSize { get; }
    public string? Password { get; }
}
