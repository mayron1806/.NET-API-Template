using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class OrganizationRepository(DatabaseContext context) : Repository<Organization, int>(context), IOrganizationRepository
    {
    }
}