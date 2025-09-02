using MediatR;
using TaskApp.Application.Dtos.Tasks;

namespace TaskApp.Application.Queries.Tasks.GetStats
{
    public sealed record GetStatsQuery(Guid UserId)
        : IRequest<TasksStasDto>;
}
