using AutoMapper;
using TaskApp.Application.Dtos;
using TaskApp.Domain.Entities;

namespace TaskApp.WepApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserCredentialsDto>()
                .ForMember(des => des.Email, opt => opt.MapFrom(src => src.Email.Value));
        }

    }
}
