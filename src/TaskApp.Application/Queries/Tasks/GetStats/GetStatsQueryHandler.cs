using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Dtos.Tasks;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Queries.Tasks.GetStats
{
    public sealed class GetStatsQueryHandler : IRequestHandler<GetStatsQuery, TasksStasDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStatsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TasksStasDto?> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _unitOfWork.TaskItemRepository
                .GetAll()
                .Where(t => t.Category.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            if (tasks == null || !tasks.Any())
                return new TasksStasDto
                {
                    TotalTasks = 0,
                    CompletedTasks = 0,
                    OverdueTasks = 0
                };

            var now = DateTime.UtcNow;

            return new TasksStasDto
            {
                TotalTasks = tasks.Count,
                CompletedTasks = tasks.Count(t => t.IsCompleted),
                OverdueTasks = tasks.Count(t => t.DueDate != null && t.DueDate.Value < now && !t.IsCompleted),
                UpComingTasks = tasks.Count(t => t.DueDate != null && t.DueDate.Value >= now && !t.IsCompleted),
                TodayTasks = tasks.Count(t => t.DueDate != null && t.DueDate.Value.Date == now.Date && !t.IsCompleted)

            };
        }

    }
}
