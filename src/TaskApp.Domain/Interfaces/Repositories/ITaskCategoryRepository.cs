using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Domain.Interfaces.Repositories
{
    public interface ITaskCategoryRepository : IRepository<TaskCategory>
    {
        Task<TaskCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
