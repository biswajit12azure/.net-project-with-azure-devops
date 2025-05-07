using Moq;
using WGL.Auth.Application.CQRS.Account.Commands.FileUpload;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.BlobFiles.FileInterface;

namespace WGL.Auth.Test.Account.Commands.MCFileUpload;
public class UploadMCUserAdditionalDataCommandHandlerTests
{
    private readonly Mock<IBlobStorageRepository> _blobStorageRepositoryMock;
    private readonly Mock<IAccountRepositoryAsync> _accountRepositoryMock;
    private readonly UploadMCUserAdditionalDataCommandHandler _handler;
    public UploadMCUserAdditionalDataCommandHandlerTests()
    {
        _blobStorageRepositoryMock = new Mock<IBlobStorageRepository>();
        _accountRepositoryMock = new Mock<IAccountRepositoryAsync>();
        _handler = new UploadMCUserAdditionalDataCommandHandler(_blobStorageRepositoryMock.Object, _accountRepositoryMock.Object);
    }
    [Fact]
    public async Task Handle_ValidFiles_ReturnsSuccessResponse()
    {
        // Arrange
        var command = new FileUploadMCUserAdditionalDataCommand
        {
            Data = new AdditionalMC
            {
                UserID = 1,
                FileData = new List<Document>
                {
                    new Document { FileName = "file1.txt", File = "dGVzdA==" },
                    new Document { FileName = "file2.txt", File = "dGVzdA==" }
                }
            }
        };
        _blobStorageRepositoryMock
            .Setup(x => x.UploadFileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<byte[]>()))
            .ReturnsAsync("http://bloburl.com/file");
        _accountRepositoryMock
            .Setup(x => x.InsertAdditionalMCUserDataAsync(It.IsAny<AdditionalMC>()))
            .ReturnsAsync(1);
        _accountRepositoryMock
            .Setup(x => x.UploadUserDocumentsAsync(It.IsAny<List<Document>>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal("insert succussfully", result.Message);
    }
    [Fact]
    public async Task Handle_NoValidFiles_ReturnsFailureResponse()
    {
        // Arrange
        var command = new FileUploadMCUserAdditionalDataCommand
        {
            Data = new AdditionalMC
            {
                UserID = 1,
                FileData = new List<Document>()
            }
        };
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("No valid files to process.", result.Message);
    }
}
