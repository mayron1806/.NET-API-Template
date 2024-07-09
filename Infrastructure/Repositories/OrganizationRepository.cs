using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class OrganizationRepository(DatabaseContext context) : Repository<Organization, int>(context), IOrganizationRepository
    {
        public void DeleteList(List<Organization> list) 
        {
            _dbSet.RemoveRange(list);
        }
    }
}