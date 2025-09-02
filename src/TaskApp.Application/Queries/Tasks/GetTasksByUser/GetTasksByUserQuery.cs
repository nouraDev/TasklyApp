using MediatR;
using TaskApp.Application.Dtos.Tasks;

namespace TaskApp.Application.Queries.Tasks.GetTasksByUser
{
    public sealed record GetTasksByUserQuery(Guid UserId)
        : IRequest<IReadOnlyList<TaskItemDto>>;
}
