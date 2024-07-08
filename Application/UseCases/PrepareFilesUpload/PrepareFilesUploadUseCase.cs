using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.PrepareFilesUpload;

public class PrepareFilesUploadUseCase(
    ILogger<PrepareFilesUploadUseCase> logger,
    IUnitOfWork unitOfWork
    ) : UseCase<PrepareFilesUploadInputDto, PrepareFilesUploadOutputDto>(logger), IPrepareFilesUpload
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public override Task<PrepareFilesUploadOutputDto> Execute(PrepareFilesUploadInputDto input)
    {
        // verificar pelo plano se pode fazer upload
        throw new NotImplementedException();
    }
}
