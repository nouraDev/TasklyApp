using AutoMapper;
using TaskApp.Application.Commands.Tasks.UpdateTaskItem;
using TaskApp.Domain.Entities;

namespace TaskApp.WepApi.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<UpdateTaskItemCommand, TaskItem>()
                .ForMember(des => des.DueDate, opt => opt.MapFrom(src => src.DueDate.Value));
        }
    }
}
