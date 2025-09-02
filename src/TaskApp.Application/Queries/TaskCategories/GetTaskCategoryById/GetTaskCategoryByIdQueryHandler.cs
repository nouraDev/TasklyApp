using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Dtos.TaskCategories;
using TaskApp.Domain.Interfaces.Geniric;


namespace TaskApp.Application.Queries.TaskCategories.GetTaskCategoryById
{
    public sealed class GetTaskCategoryByIdQueryHandler : IRequestHandler<GetTaskCategoryByIdQuery, TaskCategoryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTaskCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskCategoryDto?> Handle(GetTaskCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.TaskListRepository.GetAll().AsQueryable()
                .Where(tc => tc.Id == request.Id)
                .Select(tc => new TaskCategoryDto
                {
                    Id = tc.Id,
                    Name = tc.Name,
                    Color = tc.Color,
                    UserId = tc.UserId
                })
                .FirstOrDefaultAsync();
        }
    }
}
