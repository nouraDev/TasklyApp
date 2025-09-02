using AutoMapper;
using FluentAssertions;
using Moq;
using TaskApp.Application.Commands.Users.UserLogin;
using TaskApp.Application.Common.Errors;
using TaskApp.Application.Dtos;
using TaskApp.Application.Interfaces;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;
using TaskApp.Domain.Interfaces.Repositories;
using TaskApp.Domain.ObjectValues;
using Xunit;

namespace ApplicationTests.users
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);

            _handler = new LoginUserCommandHandler(_unitOfWorkMock.Object, _passwordHasherMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var command = new LoginUserCommand("noura@gmail.com", "password123");
            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(command.Email)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NotFoundByEmail);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ReturnsFailure()
        {
            // Arrange
            var command = new LoginUserCommand("noura@gmail.com", "wrongPassword");
            var user = new User("Noura", "hashedPassword", new Email(command.Email));

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(command.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.Verify(command.Password, user.Password)).Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.PasswordUncorrect);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsSuccessWithUserDto()
        {
            // Arrange
            var command = new LoginUserCommand("noura@gmail.com", "password1128");
            var user = new User("Noura", "hashedPassword", new Email(command.Email));
            var userDto = new UserCredentialsDto { Id = user.Id, Email = user.Email.Value };

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(command.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.Verify(command.Password, user.Password)).Returns(true);
            _mapperMock.Setup(m => m.Map<UserCredentialsDto>(user)).Returns(userDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(userDto);
        }
    }
}
