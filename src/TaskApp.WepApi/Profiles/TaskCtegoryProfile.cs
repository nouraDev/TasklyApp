using AutoMapper;
using TaskApp.Application.Commands.TaskCategories.UpdateTaskCategory;
using TaskApp.Domain.Entities;

namespace TaskApp.WepApi.Profiles
{
    public class TaskCtegoryProfile : Profile
    {
        public TaskCtegoryProfile()
        {
            CreateMap<UpdateTaskCategoryCommand, TaskCategory>();
        }
    }
}
