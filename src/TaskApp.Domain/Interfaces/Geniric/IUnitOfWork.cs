using TaskApp.Domain.Interfaces.Repositories;

namespace TaskApp.Domain.Interfaces.Geniric
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ITaskCategoryRepository TaskListRepository { get; }
        ITaskItemRepository TaskItemRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
