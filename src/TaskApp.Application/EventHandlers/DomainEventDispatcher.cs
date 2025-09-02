using Microsoft.Extensions.Logging;
using TaskApp.Domain.Events;
using TaskApp.Domain.Shared;

namespace TaskApp.Application.EventHandlers
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ILogger<DomainEventDispatcher> _logger;
        public DomainEventDispatcher(ILogger<DomainEventDispatcher> logger)
        {
            _logger = logger;
        }
        public Task Dispatch(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                switch (domainEvent)
                {
                    case TaskCompletedEvent taskCompleted:
                        _logger.LogInformation($"[Domain Event] Task {taskCompleted.TaskId} completed by {taskCompleted.CompletedByUserId}");
                        break;

                    default:
                        _logger.LogInformation($"[Domain Event] Unknown event: {domainEvent.GetType().Name}");
                        break;
                }
            }
            return Task.CompletedTask;
        }

    }
}
