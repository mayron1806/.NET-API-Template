using Domain;

namespace Application.UseCases.ConfirmFileReceive;

// lista de arquivos para fazer upload
// Name
// ContentType
// Size
public class ConfirmFileReceiveInputDto
{
    public required string TransferKey { get; set; }
}
public class ConfirmFileReceiveOutputDto
{
}
