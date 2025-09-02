using Microsoft.EntityFrameworkCore;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Repositories;
using TaskApp.Infrastructure.Persistence;

namespace TaskApp.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public async Task<bool> AnyUserWithSameEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.Value == email);
        }
        public void DeleteAsync(User entity)
        {
            _context.Users?.Remove(entity);
        }

        public IQueryable<User> GetAll()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email.Value == email, default);
        }
    }
}
