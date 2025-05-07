using Moq;
using WGL.Auth.Application.CQRS.Account.Queries.MapCentreGetUsers;
using WGL.Auth.Application.DTOs.Master;
using WGL.Auth.Application.Interfaces.Account;

namespace WGL.Auth.Test.ControllersTest.Queries.GetUserAuthToken
{
    public class GetUserMapCenterQueryHandlerTests
    {
        private readonly Mock<IAccountRepositoryAsync> _mockRepository;
        private readonly GetUserMapCenterQueryHandler _handler;
        public GetUserMapCenterQueryHandlerTests()
        {
            _mockRepository = new Mock<IAccountRepositoryAsync>();
            _handler = new GetUserMapCenterQueryHandler(_mockRepository.Object);
        }
        [Fact]
        public async Task Handle_UserExists_ReturnsSuccessfulResponse()
        {
            // Arrange
            var query = new GetUserMapCenterQuery { UserId = 1 };
            var userResponse = new UserResponseMCDTO { UserID = 1, FirstName = "John", LastName = "Doe" };
            _mockRepository.Setup(repo => repo.GetMapCenterUserByIdAsync(query)).ReturnsAsync(userResponse);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal("Successfull", result.Message);
            Assert.Equal(userResponse, result.Data);
        }
        [Fact]
        public async Task Handle_UserDoesNotExist_ReturnsFailedResponse()
        {
            // Arrange
            var query = new GetUserMapCenterQuery { UserId = 1 };
            _mockRepository.Setup(repo => repo.GetMapCenterUserByIdAsync(query)).ReturnsAsync((UserResponseMCDTO)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Equal("Failed", result.Message);
            Assert.Null(result.Data);
        }
    }
}
public class GetUserMapCenterQueryHandlerTests
{
    private readonly Mock<IAccountRepositoryAsync> _mockRepository;
    private readonly GetUserMapCenterQueryHandler _handler;

    public GetUserMapCenterQueryHandlerTests()
    {
        _mockRepository = new Mock<IAccountRepositoryAsync>();
        _handler = new GetUserMapCenterQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_UserExists_ReturnsSuccessfulResponse()
    {
        // Arrange
        var query = new GetUserMapCenterQuery { UserId = 1 };
        var userResponse = new UserResponseMCDTO { UserID = 1, FirstName = "John", LastName = "Doe" };
        _mockRepository.Setup(repo => repo.GetMapCenterUserByIdAsync(query)).ReturnsAsync(userResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Equal("Successfull", result.Message);
        Assert.Equal(userResponse, result.Data);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsFailedResponse()
    {
        // Arrange
        var query = new GetUserMapCenterQuery { UserId = 1 };
        _mockRepository.Setup(repo => repo.GetMapCenterUserByIdAsync(query)).ReturnsAsync((UserResponseMCDTO)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Succeeded);
        Assert.Equal("Failed", result.Message);
        Assert.Null(result.Data);
    }
}
