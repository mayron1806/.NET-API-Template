using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class FileRepository(DatabaseContext context) : Repository<Domain.File, int>(context), IFileRepository
    {
    }
}