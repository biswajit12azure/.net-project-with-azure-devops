using Moq;
using WGL.Auth.Application.CQRS.Account.Queries.GetUserAuthToken;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Exceptions;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Test.ControllersTest.Queries.GetUserAuthToken
{
    public class GetUserAuthTokenQueryHandlerTests
    {
        private readonly Mock<IAccountRepositoryAsync> _mockAccountRepository;
        private readonly GetUserAuthTokenQueryHandler _handler;
        public GetUserAuthTokenQueryHandlerTests()
        {
            _mockAccountRepository = new Mock<IAccountRepositoryAsync>();
            _handler = new GetUserAuthTokenQueryHandler(_mockAccountRepository.Object);
        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsAuthenticationResponse()
        {
            // Arrange
            var request = new GetUserAuthTokenQuery { UserName = "testuser", Password = "password" };
            var authResponse = new AuthenticationResponse { UserDetails = new Userdetails() };
            var response = new Response<AuthenticationResponse>(authResponse);
            _mockAccountRepository.Setup(repo => repo.GenerateUserTokenQuery(request.UserName, request.Password))
                .ReturnsAsync(response);
            // Act
            var result = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(authResponse, result.Data);
        }
        [Fact]
        public async Task Handle_InvalidUser_ThrowsApiException()
        {
            // Arrange
            var request = new GetUserAuthTokenQuery { UserName = "invaliduser", Password = "password" };
            var response = new Response<AuthenticationResponse>(new AuthenticationResponse());
            _mockAccountRepository.Setup(repo => repo.GenerateUserTokenQuery(request.UserName, request.Password))
                .ReturnsAsync(response);
            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}
public class GetUserAuthTokenQueryHandlerTests
{
    private readonly Mock<IAccountRepositoryAsync> _mockAccountRepository;
    private readonly GetUserAuthTokenQueryHandler _handler;

    public GetUserAuthTokenQueryHandlerTests()
    {
        _mockAccountRepository = new Mock<IAccountRepositoryAsync>();
        _handler = new GetUserAuthTokenQueryHandler(_mockAccountRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsAuthenticationResponse()
    {
        // Arrange
        var request = new GetUserAuthTokenQuery { UserName = "testuser", Password = "password" };
        var authResponse = new AuthenticationResponse { UserDetails = new Userdetails() };
        var response = new Response<AuthenticationResponse>(authResponse);
        _mockAccountRepository.Setup(repo => repo.GenerateUserTokenQuery(request.UserName, request.Password))
            .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Equal(authResponse, result.Data);
    }

    [Fact]
    public async Task Handle_InvalidUser_ThrowsApiException()
    {
        // Arrange
        var request = new GetUserAuthTokenQuery { UserName = "invaliduser", Password = "password" };
        var response = new Response<AuthenticationResponse>(new AuthenticationResponse());
        _mockAccountRepository.Setup(repo => repo.GenerateUserTokenQuery(request.UserName, request.Password))
            .ReturnsAsync(response);

        // Act & Assert
        await Assert.ThrowsAsync<ApiException>(() => _handler.Handle(request, CancellationToken.None));
    }
}
