using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Dtos.TaskCategories;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Queries.TaskCategories.GetTaskCategoriesByUser
{
    public class GetTaskCategoriesByUserQueryHandler
        : IRequestHandler<GetTaskCategoriesByUserQuery, IReadOnlyList<TaskCategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTaskCategoriesByUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<TaskCategoryDto>> Handle(GetTaskCategoriesByUserQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.TaskListRepository.GetAll()
                .Where(tc => tc.UserId == request.UserId)
                .Select(tc => new TaskCategoryDto
                {
                    Id = tc.Id,
                    Name = tc.Name,
                    Color = tc.Color,
                    UserId = tc.UserId,
                    TasksCount = tc.Tasks.Count
                })
                .ToListAsync(cancellationToken);
        }
    }
}
