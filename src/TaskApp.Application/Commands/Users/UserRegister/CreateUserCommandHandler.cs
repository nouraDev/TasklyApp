using MediatR;
using TaskApp.Application.Common;
using TaskApp.Application.Common.Errors;
using TaskApp.Application.Interfaces;
using TaskApp.Domain.Interfaces.Geniric;
using TaskApp.Domain.ObjectValues;

namespace TaskApp.Application.Commands.Users.UserRegister;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Response<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.UserRepository.AnyUserWithSameEmailAsync(request.Email))
        {
            return Response.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        var email = new Email(request.Email);
        var passwordHashed = _passwordHasher.Hash(request.Password);

        var user = new Domain.Entities.User(request.Name, passwordHashed, email);

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(user.Id);
    }
}
