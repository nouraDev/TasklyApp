using MediatR;
using TaskApp.Application.Common;
using TaskApp.Domain.Enums;

namespace TaskApp.Application.Commands.Tasks.CreateTaskItem
{
    public record CreateTaskItemCommand(
        string Title,
        string Description,
        PriorityCategory PriorityCategory,
        Guid ListId,
        Guid UserId,
        DateTime? DueDate
    ) : IRequest<Response<Guid>>;
}
