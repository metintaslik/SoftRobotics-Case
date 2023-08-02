using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.ProfileHandler.Command;
using softrobotics.auth.domain.Entity;
using System.Linq.Expressions;

namespace softrobotics.auth.test
{
    public class UserDeActivateCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenValidRequest_ShouldDeactivateUser()
        {
            // Arrange
            var contextMock = new Mock<ISoftRoboticsDbContext>();
            var tokenHelperMock = new Mock<ITokenHelper>();

            var user = new User
            {
                UserId = 1,
                IsActive = true // Assuming user is active initially
            };

            contextMock.Setup(c => c.Users.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var command = new UserDeActivateCommand
            {
                IsActive = false
            };

            var handler = new UserDeActivateCommandHandler(contextMock.Object, tokenHelperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            user.IsActive.Should().BeFalse(); // Check if user is deactivated
            contextMock.Verify(c => c.SaveToDbAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}