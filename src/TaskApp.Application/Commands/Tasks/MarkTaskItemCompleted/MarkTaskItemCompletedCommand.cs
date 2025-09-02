using MediatR;
using TaskApp.Application.Common;

namespace TaskApp.Application.Commands.Tasks.MarkTaskItemCompleted
{
    public record MarkTaskItemCompletedCommand(Guid TaskId, Guid UserId) : IRequest<Response>;
}
