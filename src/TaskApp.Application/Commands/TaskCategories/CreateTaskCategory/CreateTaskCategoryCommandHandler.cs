using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskApp.Application.Common;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Commands.TaskCategories.CreateTaskCategory
{
    public sealed class CreateTaskCategoryCommandHandler : IRequestHandler<CreateTaskCategoryCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateTaskCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<Guid>> Handle(CreateTaskCategoryCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _unitOfWork.UserRepository
                .GetAll()
                .AnyAsync(u => u.Id == request.UserId);

            if (!userExists)
                return Response.Failure<Guid>(Error.NotFound("User.NotFound", "User does not exist."));

            var alreadyExists = await _unitOfWork.TaskListRepository
                .GetAll()
                .AnyAsync(c => c.UserId == request.UserId && c.Name == request.Name);

            if (alreadyExists)
                return Response.Failure<Guid>(Error.Conflict("Category.Duplicate", "Category with the same name already exists."));

            var category = new Domain.Entities.TaskCategory(request.Name, request.UserId, request.Color);

            await _unitOfWork.TaskListRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(category.Id);
        }
    }
}
