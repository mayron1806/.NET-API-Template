namespace Application.UseCases.PrepareFilesUpload;

// lista de arquivos para fazer upload
// Name
// ContentType
// Size
public class PrepareFilesUploadInputDto
{
    public int UserId { get; set; }
    public int OrganizationId { get; set; }
    public IEnumerable<FileUpload> Files { get; set; } = [];

    public class FileUpload {

        public required string Name { get; set; }
        public required string ContentType { get; set; }
        public long Size { get; set; }
    }
}
public class PrepareFilesUploadOutputDto
{
    
}
