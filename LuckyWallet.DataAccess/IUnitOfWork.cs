using Microsoft.EntityFrameworkCore.Storage;

namespace LuckyWallet.DataAccess;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken cancellationToken = default);
    void Save();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
