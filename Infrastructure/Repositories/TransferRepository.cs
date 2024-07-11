using Domain;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransferRepository(DatabaseContext context) : Repository<Transfer, int>(context), ITransferRepository
{
    public async Task<IEnumerable<Transfer>> GetExpiredTransfers() 
    {
        var query = _dbSet.Where(x => x.ExpiresAt < DateTime.UtcNow && !x.Expired);
        return await query.AsNoTracking().ToListAsync();
    }
    public void SetAsExpired(Transfer transfer) {
        transfer.SetAsExpired();
        Update(transfer);
    }
}