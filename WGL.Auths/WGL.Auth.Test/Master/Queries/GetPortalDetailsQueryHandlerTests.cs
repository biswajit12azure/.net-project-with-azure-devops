using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WGL.Auth.Application.CQRS.Masters.Queries;
using WGL.Auth.Application.DTOs.Master;
using WGL.Auth.Application.Interfaces.Master;
using WGL.Utility.Wrappers;
using Xunit;

public class GetPortalDetailsQueryHandlerTests
{
    private readonly Mock<IPortalDetailsRepository> _mockPortalDetailsRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetPortalDetailsQueryHandler _handler;

    public GetPortalDetailsQueryHandlerTests()
    {
        _mockPortalDetailsRepository = new Mock<IPortalDetailsRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetPortalDetailsQueryHandler(_mockPortalDetailsRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ReturnsPortalDetails()
    {
        // Arrange
        var portalDetails = new List<PortalDTO>
        {
            new PortalDTO { PortalID = 1, PortalName = "Portal1", PortalDescription = "Description1", PortalKey = "Key1" },
            new PortalDTO { PortalID = 2, PortalName = "Portal2", PortalDescription = "Description2", PortalKey = "Key2" }
        };
        _mockPortalDetailsRepository.Setup(repo => repo.GetPortalDetails()).ReturnsAsync(portalDetails);

        var query = new GetPortalDetailsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Equal("Dcoument Type Received Successfully!!", result.Message);
        Assert.Equal(portalDetails, result.Data);
    }
}
