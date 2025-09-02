using MediatR;
using TaskApp.Application.Common;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Commands.TaskCategories.DeleteTaskCategory
{
    public sealed class DeleteTaskCategoryCommandHandler : IRequestHandler<DeleteTaskCategoryCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteTaskCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> Handle(DeleteTaskCategoryCommand request, CancellationToken cancellationToken)
        {
            TaskCategory? category = await _unitOfWork.TaskListRepository
                                .GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                return Response.Failure<Guid>(Error.NotFound("Category.NotFound", "Category does not exist."));

            _unitOfWork.TaskListRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Response.Success("Task category updated successfully.");
        }
    }
}
