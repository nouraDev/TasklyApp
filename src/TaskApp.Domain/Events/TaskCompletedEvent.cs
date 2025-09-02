using TaskApp.Domain.Shared;

namespace TaskApp.Domain.Events
{
    public record TaskCompletedEvent(Guid TaskId, Guid CompletedByUserId) : IDomainEvent;

}
