using MediatR;
using TaskApp.Application.Common;

namespace TaskApp.Application.Commands.Tasks.DeleteTaskItem
{
    public record DeleteTaskItemCommand(Guid TaskId) : IRequest<Response>;
}
