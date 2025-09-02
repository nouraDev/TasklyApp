using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Repositories;
using TaskApp.Infrastructure.Persistence;


namespace TaskApp.Infrastructure.Repositories
{
    public sealed class TaskCategoryRepository : ITaskCategoryRepository
    {
        private readonly AppDbContext _context;
        public TaskCategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TaskCategory entity)
        {
            await _context.TaskCategories.AddAsync(entity);
        }
        public void DeleteAsync(TaskCategory entity)
        {
            _context.TaskCategories?.Remove(entity);
        }

        public IQueryable<TaskCategory> GetAll()
        {
            return _context.TaskCategories;
        }

        public async Task<TaskCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.TaskCategories.FindAsync(id, cancellationToken);
        }
    }
}
