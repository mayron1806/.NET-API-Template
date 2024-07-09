using Application.Exceptions;
using Application.Services.PlanService;
using Domain;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.GetOrganizationByUser;

public class GetOrganizationByUserUseCase(
    ILogger<GetOrganizationByUserUseCase> logger,
    IUnitOfWork unitOfWork,
    IPlanService planService
    ) : UseCase<GetOrganizationByUserInputDto, GetOrganizationByUserOutputDto>(logger), IGetOrganizationByUserUseCase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPlanService _planService = planService;

    public override async Task<GetOrganizationByUserOutputDto> Execute(GetOrganizationByUserInputDto input)
    {
        var user = await _unitOfWork.User.GetByIdAsync(input.UserId, "Members");
        if (user == null) throw new HttpException(400, "Usuário nao encontrado.");
        if (user.Members == null || user.Members.ToList().Count == 0) throw new HttpException(400, "Usuário não possui uma organização.");
        // Usuario não pode ter mais de 1 organização, se tiver deve deletar
        if (user.Members?.ToList().Count > 1) {
            throw new HttpException(400, "Usuário não pode ter mais de 1 organização.");
        }
        
        var member = user.Members!.ToList()[0];
        var organizationId = member.OrganizationId;
        var organization = await _unitOfWork.Organization.GetByIdAsync(organizationId);
        if (organization == null) throw new HttpException(400, "Organizacao nao encontrada");
        var plan = _planService.GetPlanByOrganization(organization);
        return new GetOrganizationByUserOutputDto(organizationId, member.IsOwner, plan, organization.PlanActive);
    }
}
