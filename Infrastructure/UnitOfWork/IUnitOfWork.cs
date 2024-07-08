using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    IUserRepository User { get; }
    IActiveAccountTokenRepository ActiveAccountToken { get; }
    IResetPasswordTokenRepository ResetPasswordToken { get; }
    IFileRepository File { get; }
    ITransferRepository Transfer { get; }
    IOrganizationRepository Organization { get; }
    IMemberRepository Member { get; }
}