using MediatR;
using TaskApp.Application.Common;
using TaskApp.Domain.Enums;

namespace TaskApp.Application.Commands.Tasks.UpdateTaskItem
{
    public record UpdateTaskItemCommand(
        Guid TaskId,
        string Title,
        string Description,
        PriorityCategory PriorityCategory,
        DateTime? DueDate
    ) : IRequest<Response>;
}
