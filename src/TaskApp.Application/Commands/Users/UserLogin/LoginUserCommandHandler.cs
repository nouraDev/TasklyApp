using AutoMapper;
using MediatR;
using TaskApp.Application.Common;
using TaskApp.Application.Common.Errors;
using TaskApp.Application.Dtos;
using TaskApp.Application.Interfaces;
using TaskApp.Domain.Interfaces.Geniric;

namespace TaskApp.Application.Commands.Users.UserLogin
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<UserCredentialsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<Response<UserCredentialsDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);

            if (user is null)
            {
                return Response.Failure<UserCredentialsDto>(UserErrors.NotFoundByEmail);
            }
            if (!_passwordHasher.Verify(request.Password, user.Password))
            {
                return Response.Failure<UserCredentialsDto>(UserErrors.PasswordUncorrect);
            }

            var userCredentials = _mapper.Map<UserCredentialsDto>(user);

            return Response.Success(userCredentials);
        }
    }
}
