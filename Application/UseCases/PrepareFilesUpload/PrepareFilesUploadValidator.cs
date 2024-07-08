using FluentValidation;

namespace Application.UseCases.PrepareFilesUpload;

public class PrepareFilesUploadValidator: AbstractValidator<PrepareFilesUploadInputDto>
{
    public PrepareFilesUploadValidator()
    {
        RuleFor(x => x.Files).NotEmpty().WithMessage("O campo arquivos é obrigatório");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("O usuário deve ser informado");

        RuleForEach(x => x.Files).SetValidator(new PrepareFilesUploadFileValidator());
    }
}
class PrepareFilesUploadFileValidator: AbstractValidator<PrepareFilesUploadInputDto.FileUpload>
{
    public PrepareFilesUploadFileValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("O campo nome é obrigatório");
        RuleFor(x => x.ContentType).NotEmpty().WithMessage("O campo tipo é obrigatório");
        RuleFor(x => x.Size).NotEmpty().WithMessage("O campo tamanho é obrigatório");
    }
}