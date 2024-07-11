using Domain;

namespace Infrastructure.Repositories.Interfaces;

public interface ITransferRepository : IRepository<Transfer, int> {
    Task<IEnumerable<Transfer>> GetExpiredTransfers();
    void SetAsExpired(Transfer transfer);
}
