using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WGL.Auth.Application.CQRS.Account.Queries.GetUserProfileByPortalID;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Wrappers;
using Xunit;

namespace WGL.Auth.Application.Tests.CQRS.Account.Queries.GetUserProfileByPortalID
{
    public class GetUserProfileByPortalIDHandlerTests
    {
        private readonly Mock<IAccountRepositoryAsync> _mockAccountRepository;
        private readonly GetUserProfileByPortalIDHandlerQueryHandler _handler;

        public GetUserProfileByPortalIDHandlerTests()
        {
            _mockAccountRepository = new Mock<IAccountRepositoryAsync>();
            _handler = new GetUserProfileByPortalIDHandlerQueryHandler(_mockAccountRepository.Object);
        }

        [Fact]
        public async Task Handle_ReturnsUserProfileResponseList()
        {
            // Arrange
            var portalID = 1;
            var request = new GetUserProfileByPortalIDHandler { PortalID = portalID };
            var userProfileResponseList = new List<UserProfileResponse>
            {
                new UserProfileResponse
                {
                    User = new List<UserRequestParms>
                    {
                        new UserRequestParms { UserId = 1, EmailID = "test@example.com", Status = "Active", AgencyID = 1, RoleID = 1 }
                    },
                    Agency = new List<AgencyRequestParms>(),
                    Roles = new List<RoleRequestParms>(),
                    Marketer = new List<MarketerRequestParms>()
                }
            };

            _mockAccountRepository.Setup(repo => repo.GetUserProfileByPortalID(portalID))
                .ReturnsAsync(userProfileResponseList);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(userProfileResponseList, result.Data);
        }
    }
}
