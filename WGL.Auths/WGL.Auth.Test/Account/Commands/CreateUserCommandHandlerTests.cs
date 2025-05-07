using AutoMapper;
using Moq;
using WGL.Auth.Application.CQRS.Account.Commands.CreateUser;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.EmailService;

namespace WGL.Auth.Test.Account.commands.CreateUser;
public class CreateUserCommandHandlerTests
{
    private readonly Mock<IAccountRepositoryAsync> _accountRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IEmailServiceAsync> _emailServiceMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepositoryAsync>();
        _mapperMock = new Mock<IMapper>();
        _emailServiceMock = new Mock<IEmailServiceAsync>();
        _handler = new CreateUserCommandHandler(_accountRepositoryMock.Object, _mapperMock.Object, _emailServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenUserIsCreated()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            FullName = "John Doe",
            CompanyName = "Company",
            MobileNumber = "1234567890",
            EmailAddress = "john.doe@example.com",
            Password = "password",
            PortalId = 1
        };

        var applicationUser = new ApplicationUser
        {
            CompanyName = command.CompanyName,
            MobileNumber = command.MobileNumber,
            FirstName = command.FirstName,
            LastName = command.LastName,
            FullName = command.FullName,
            Password = "encodedPassword",
            EmailID = command.EmailAddress,
            CreatedBy = 0,
            PortalId = command.PortalId
        };

        _mapperMock.Setup(m => m.Map<ApplicationUser>(It.IsAny<ApplicationUser>())).Returns(applicationUser);
        _accountRepositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("newUserId");
        _emailServiceMock.Setup(e => e.SendAsync(It.IsAny<EmailRequest>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("newUserId", result.Data);
        Assert.Equal("New User Created", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnErrorResponse_WhenEmailAlreadyExists()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            FullName = "John Doe",
            CompanyName = "Company",
            MobileNumber = "1234567890",
            EmailAddress = "john.doe@example.com",
            Password = "password",
            PortalId = 1
        };

        var applicationUser = new ApplicationUser
        {
            CompanyName = command.CompanyName,
            MobileNumber = command.MobileNumber,
            FirstName = command.FirstName,
            LastName = command.LastName,
            FullName = command.FullName,
            Password = "encodedPassword",
            EmailID = command.EmailAddress,
            CreatedBy = 0,
            PortalId = command.PortalId
        };

        _mapperMock.Setup(m => m.Map<ApplicationUser>(It.IsAny<ApplicationUser>())).Returns(applicationUser);
        _accountRepositoryMock.Setup(r => r.CreateUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("EmaildExist");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("EmaildExist", result.Data);
        Assert.Equal("Email Already Exist", result.Message);
    }
}