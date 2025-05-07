using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using Moq;
using WGL.Auth.Application.CQRS.Account.Queries.GetUserLoginToken;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Wrappers;
using Xunit;

namespace WGL.Auth.Test.ControllersTest.Queries.GetUserAuthToken;
public class GenerateUserLoginTokenQueryHandlerTests
{
    private readonly Mock<ICacheProvider> _cacheProviderMock;
    private readonly Mock<IAccountRepositoryAsync> _accountRepositoryMock;
    private readonly GenerateUserLoginTokenQueryHandler _handler;

    public GenerateUserLoginTokenQueryHandlerTests()
    {
        _cacheProviderMock = new Mock<ICacheProvider>();
        _accountRepositoryMock = new Mock<IAccountRepositoryAsync>();
        _handler = new GenerateUserLoginTokenQueryHandler(_cacheProviderMock.Object, _accountRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new GenerateUserLoginTokenQuery { UserName = "testuser", Password = "password" };
        var authResponse = new AuthenticationResponse { UserDetails = new Userdetails { jwToken = "testtoken" } };
        var response = new Response<AuthenticationResponse>(authResponse, "Token Received");

        _accountRepositoryMock.Setup(x => x.GenerateUserTokenQuery(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Equal("Token Received", result.Message);
        Assert.Equal("testtoken", result.Data.UserDetails.jwToken);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new GenerateUserLoginTokenQuery { UserName = "invaliduser", Password = "invalidpassword" };
        var response = new Response<AuthenticationResponse>(null, "Invalid credentials");

        _accountRepositoryMock.Setup(x => x.GenerateUserTokenQuery(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Succeeded);
        Assert.Equal("Invalid credentials", result.Message);
        Assert.Null(result.Data);
    }
}