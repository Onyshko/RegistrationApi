using RegApi.Domain.Entities;

namespace RegApi.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        IUserAccountRepository UserAccountRepository();
        IUSerRepository UserRepository();
        Task SaveChangesAsync();
    }
}
