using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> AnyUserWithSameEmailAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
