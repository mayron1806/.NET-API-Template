namespace Domain;

public class Transfer
{
    public Transfer(string key, int organizationId, DateTime expiresAt, int size, string path, TransferType transferType)
    {
        Key = key;
        OrganizationId = organizationId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        Size = size;
        Path = path;
        TransferType = transferType;
    }
    public int Id { get; }
    public string Key { get; }
    public IEnumerable<File>? Files { get; }
    public Organization? Organization { get; }
    public int OrganizationId { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }
    public int Size { get; }
    public int FilesCount { get; }
    public string? Path { get; }
    public TransferType TransferType { get; }
    public Receive? Receive { get; }
    public Send? Send { get; }
}
