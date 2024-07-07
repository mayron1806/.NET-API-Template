using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        IUserRepository User { get; }
    }
}