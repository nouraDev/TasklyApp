using FluentAssertions;
using Moq;
using TaskApp.Application.Commands.Users.UserRegister;
using TaskApp.Application.Common.Errors;
using TaskApp.Application.Interfaces;
using TaskApp.Domain.Entities;
using TaskApp.Domain.Interfaces.Geniric;
using TaskApp.Domain.Interfaces.Repositories;
using Xunit;

namespace ApplicationTests.users
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);

            _handler = new CreateUserCommandHandler(_unitOfWorkMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Handle_EmailAlreadyExists_ReturnsFailure()
        {
            // Arrange
            var command = new CreateUserCommand("Noura", "noura@gmail.com", "password1128");
            _userRepositoryMock.Setup(r => r.AnyUserWithSameEmailAsync(command.Email)).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.EmailNotUnique);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidRequest_CreatesUserAndReturnsSuccess()
        {
            // Arrange
            var command = new CreateUserCommand("John Doe", "john@example.com", "password123");
            _userRepositoryMock.Setup(r => r.AnyUserWithSameEmailAsync(command.Email)).ReturnsAsync(false);
            _passwordHasherMock.Setup(h => h.Hash(command.Password)).Returns("hashedPassword");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBe(Guid.Empty);

            _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u =>
                u.Name == command.Name &&
                u.Email.Value == command.Email &&
                u.Password == "hashedPassword")), Times.Once);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
