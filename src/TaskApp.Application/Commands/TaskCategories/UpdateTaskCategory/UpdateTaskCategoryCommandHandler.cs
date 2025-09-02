using AutoMapper;
using MediatR;
using TaskApp.Application.Common;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Commands.TaskCategories.UpdateTaskCategory
{
    public sealed class UpdateTaskCategoryCommandHandler : IRequestHandler<UpdateTaskCategoryCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateTaskCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response> Handle(UpdateTaskCategoryCommand request, CancellationToken cancellationToken)
        {
            TaskCategory? category = await _unitOfWork.TaskListRepository
                                            .GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                return Response.Failure<Guid>(Error.NotFound("Category.NotFound", "Category does not exist."));

            _mapper.Map(request, category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Response.Success("Task category updated successfully.");

        }
    }
}
