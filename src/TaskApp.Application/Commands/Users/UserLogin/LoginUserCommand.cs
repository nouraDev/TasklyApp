using MediatR;
using TaskApp.Application.Common;
using TaskApp.Application.Dtos;

namespace TaskApp.Application.Commands.Users.UserLogin
{
    public record LoginUserCommand(string Email, string Password) : IRequest<Response<UserCredentialsDto>>;

}
