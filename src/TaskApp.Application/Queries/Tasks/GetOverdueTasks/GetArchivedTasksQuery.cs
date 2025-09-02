using MediatR;
using TaskApp.Application.Dtos.Tasks;

namespace TaskApp.Application.Queries.Tasks.GetOverdueTasks
{
    public sealed record GetArchivedTasksQuery(Guid UserId)
        : IRequest<IReadOnlyList<TaskItemDto>>;
}
