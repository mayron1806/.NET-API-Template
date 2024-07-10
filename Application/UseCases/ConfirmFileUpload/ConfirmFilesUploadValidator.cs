using FluentValidation;

namespace Application.UseCases.ConfirmFilesUpload;

public class ConfirmFilesUploadValidator: AbstractValidator<ConfirmFilesUploadInputDto>
{
    public ConfirmFilesUploadValidator()
    {
        RuleForEach(x => x.TransferKey)
            .NotEmpty().WithMessage("Transferencia invalida");
    }
}