using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Repositories;
using TaskApp.Infrastructure.Persistence;

namespace TaskApp.Infrastructure.Repositories
{
    public sealed class TaskRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TaskItem entity)
        {
            await _context.TaskItems.AddAsync(entity);
        }

        public void DeleteAsync(TaskItem entity)
        {
            _context.TaskItems.Remove(entity);

        }

        public IQueryable<TaskItem> GetAll()
        {
            return _context.TaskItems.AsQueryable();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid taskId)
        {
            return await _context.TaskItems.FindAsync(taskId);
        }
    }
}
