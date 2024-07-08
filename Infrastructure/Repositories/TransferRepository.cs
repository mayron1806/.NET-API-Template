using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class TransferRepository(DatabaseContext context) : Repository<Transfer, int>(context), ITransferRepository
    {
    }
}