using MediatR;
using TaskApp.Application.Dtos.Tasks;

namespace TaskApp.Application.Queries.Tasks.GetCompletedTasks
{
    public sealed record GetCompletedTasksQuery(Guid UserId)
        : IRequest<IReadOnlyList<TaskItemDto>>;
}
