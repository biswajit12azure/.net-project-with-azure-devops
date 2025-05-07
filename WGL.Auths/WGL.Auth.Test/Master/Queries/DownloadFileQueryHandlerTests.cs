using Moq;
using System.Threading;
using System.Threading.Tasks;
using WGL.Auth.Application.CQRS.Masters.Queries;
using WGL.Utility.BlobFiles.FileInterface;
using Xunit;

public class DownloadFileQueryHandlerTests
{
    private readonly Mock<IBlobStorageRepository> _blobStorageRepositoryMock;
    private readonly DownloadFileQueryHandler _handler;

    public DownloadFileQueryHandlerTests()
    {
        _blobStorageRepositoryMock = new Mock<IBlobStorageRepository>();
        _handler = new DownloadFileQueryHandler(_blobStorageRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBlobDownloadFileResponse()
    {
        // Arrange
        var fileName = "testfile.txt";
        var fileData = new byte[] { 1, 2, 3, 4 };
        _blobStorageRepositoryMock.Setup(repo => repo.DownloadFileAsync(fileName)).ReturnsAsync(fileData);

        var query = new DownloadFileQuery(fileName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fileName, result.FileName);
        Assert.Equal("txt", result.Format);
        Assert.Equal(Convert.ToBase64String(fileData), result.File);
    }
}
