namespace TaskApp.Domain.Interfaces.Geniric
{
    public interface IRepository<T> where T : class
    {
        void DeleteAsync(T entity);
        Task AddAsync(T entity);
        IQueryable<T> GetAll();
    }
}
