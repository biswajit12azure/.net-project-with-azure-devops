using Moq;
using WGL.Auth.Application.CQRS.Account.Commands.FileUpload;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.BlobFiles.FileInterface;

namespace WGL.Auth.Test.Account.Commands.SDFileUpload;
public class UploadSDUserAdditionalDataCommandHandlerTests
{
    private readonly Mock<IBlobStorageRepository> _mockBlobStorageRepository;
    private readonly Mock<IAccountRepositoryAsync> _mockAccountRepository;
    private readonly UploadSDUserAdditionalDataCommandHandler _handler;

    public UploadSDUserAdditionalDataCommandHandlerTests()
    {
        _mockBlobStorageRepository = new Mock<IBlobStorageRepository>();
        _mockAccountRepository = new Mock<IAccountRepositoryAsync>();
        _handler = new UploadSDUserAdditionalDataCommandHandler(_mockBlobStorageRepository.Object, _mockAccountRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidFiles_ReturnsSuccessResponse()
    {
        // Arrange
        var command = new FileUploadSDUserAdditionalDataCommand
        {
            Data = new AdditionalSD
            {
                UserID = 1,
                FileData = new List<Document>
                {
                    new Document { FileName = "file1.txt", File = "dGVzdA==" },
                    new Document { FileName = "file2.txt", File = "dGVzdA==" }
                }
            }
        };

        _mockBlobStorageRepository.Setup(x => x.UploadFileAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<byte[]>()))
            .ReturnsAsync("http://blobstorage.com/file1.txt");

        _mockAccountRepository.Setup(x => x.InsertAdditionalSDUserDataAsync(It.IsAny<AdditionalSD>()))
            .ReturnsAsync(1);

        _mockAccountRepository.Setup(x => x.UploadUserDocumentsAsync(It.IsAny<List<Document>>(), It.IsAny<int>()))
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
        var command = new FileUploadSDUserAdditionalDataCommand
        {
            Data = new AdditionalSD
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

 