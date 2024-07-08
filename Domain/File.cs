namespace Domain;

public class File
{
    public File(string originalName, string path, int size, string contentType)
    {
        OriginalName = originalName;
        Path = path;
        Size = size;
        ContentType = contentType;
    }
    public int Id { get; }
    public string OriginalName { get; private set; }
    public string Path { get; private set; }
    public int Size { get; private set; }
    public string ContentType { get; private set; }
    public Transfer? Transfer { get; private set; }
    public int TransferId { get; private set; }
}
