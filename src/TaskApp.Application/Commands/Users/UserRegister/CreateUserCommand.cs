using MediatR;
using TaskApp.Application.Common;

namespace TaskApp.Application.Commands.Users.UserRegister
{
    public record CreateUserCommand(string Name, string Email, string Password) : IRequest<Response<Guid>>;
}
