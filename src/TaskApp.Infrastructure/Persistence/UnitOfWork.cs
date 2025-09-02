using TaskApp.Domain.Interfaces.Geniric;
using TaskApp.Domain.Interfaces.Repositories;

namespace TaskApp.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository UserRepository { get; }
        public ITaskCategoryRepository TaskListRepository { get; }
        public ITaskItemRepository TaskItemRepository { get; }

        public UnitOfWork(AppDbContext context,
                          IUserRepository users,
                          ITaskCategoryRepository taskLists,
                          ITaskItemRepository taskItems)
        {
            _context = context;
            UserRepository = users;
            TaskListRepository = taskLists;
            TaskItemRepository = taskItems;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
