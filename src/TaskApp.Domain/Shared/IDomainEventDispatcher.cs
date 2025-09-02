namespace TaskApp.Domain.Shared
{
    public interface IDomainEventDispatcher
    {
        public Task Dispatch(IEnumerable<IDomainEvent> domainEvents);
    }
}
