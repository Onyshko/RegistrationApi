using RegApi.Domain.Entities;

namespace RegApi.Repository.Interfaces
{
    public interface IUSerRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetAsync(string id);
        Task<User?> UpdateAsync(User user);

    }
}
