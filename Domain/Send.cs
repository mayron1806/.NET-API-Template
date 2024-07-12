﻿namespace Domain;

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
    public string? Message { get; private set; }
    public string? Password { get; private set; }
    public int Downloads { get; private set; }
    public bool QuickDownload { get; private set; }
    public bool ExpiresOnDowload { get; private set; }
    public IEnumerable<string>? Destination { get; private set; }

    public void IncrementDownload() => Downloads++;
}
