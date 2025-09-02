using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Domain.Interfaces.Repositories
{
    public interface ITaskItemRepository : IRepository<TaskItem>
    {
        Task<TaskItem?> GetByIdAsync(Guid taskId);
    }
}
