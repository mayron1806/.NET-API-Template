using Domain;

namespace Application.UseCases.ConfirmFilesUpload;

// lista de arquivos para fazer upload
// Name
// ContentType
// Size
public class ConfirmFilesUploadInputDto
{
    public required string TransferKey { get; set; }
}
public class ConfirmFilesUploadOutputDto
{
}
