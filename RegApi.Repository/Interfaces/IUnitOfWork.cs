namespace RegApi.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IUserRepository UserRepository();
        void SaveChanges();
    }
}
