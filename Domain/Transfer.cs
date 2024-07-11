﻿namespace Domain;

public class Transfer
{
    public Transfer(string key, int organizationId, DateTime expiresAt, long size, string path, TransferType transferType)
    {
        Key = key;
        OrganizationId = organizationId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        Size = size;
        Path = path;
        TransferType = transferType;
    }
    public Transfer(string key, int organizationId, DateTime expiresAt, long size, TransferType transferType)
    {
        Key = key;
        OrganizationId = organizationId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        Size = size;
        Path = $"{organizationId}/files/{key}";
        TransferType = transferType;
    }
    public int Id { get; }
    public string Key { get; }
    public IEnumerable<File>? Files { get; private set; }
    public Organization? Organization { get; }
    public int OrganizationId { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; private set; }
    public bool Expired { get; set; }
    public long Size { get; private set; }
    public int FilesCount { get; private set; }
    public string Path { get; private set; }
    public TransferType TransferType { get; private set; }
    public Receive? Receive { get; private set; }
    public Send? Send { get; private set; }
    public void AddFile(File file) {
        var files = Files?.ToList() ?? [];
        files.Add(file);
        Files = files;
        FilesCount = Files.Count();
    }

    public void SetAsExpired() {
        Expired = true;
    }

    public void AddReceive(Receive receive) => Receive = receive;
    public void AddSend(Send send) => Send = send;
}
