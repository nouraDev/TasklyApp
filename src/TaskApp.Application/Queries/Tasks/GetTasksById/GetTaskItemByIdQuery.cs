using MediatR;
using TaskApp.Application.Dtos.Tasks;

namespace TaskApp.Application.Queries.Tasks.GetTasksById
{
    public sealed record GetTaskItemByIdQuery(Guid TaskId)
        : IRequest<TaskItemDto>;
}
