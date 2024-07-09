using Domain.Plan;

namespace Application.UseCases.GetOrganizationByUser;

public class GetOrganizationByUserInputDto
{
    public int UserId { get; set; }
}
public class GetOrganizationByUserOutputDto(int organizationId, bool userIsOwner, Plan plan, bool planActive)
{
    public int OrganizationId { get; set; } = organizationId;
    public bool UserIsOwner { get; set; } = userIsOwner;
    public Plan Plan { get; set; } = plan;
    public bool PlanActive { get; set; } = planActive;
}