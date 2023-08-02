using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.ProfileHandler.Command;
using softrobotics.auth.domain.Entity;
using System.Linq.Expressions;

namespace softrobotics.auth.test;

public class UserUpdateCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenValidRequest_ShouldUpdateUser()
    {
        // Arrange
        var contextMock = new Mock<ISoftRoboticsDbContext>();
        var tokenHelperMock = new Mock<ITokenHelper>();

        var user = new User
        {
            UserId = 1,
            Username = "oldUsername",
            Mail = "old@mail.com"
        };

        contextMock.Setup(c => c.Users.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var command = new UserUpdateCommand
        {
            Username = "newUsername",
            Mail = "new@mail.com"
        };

        var handler = new UserUpdateCommandHandler(contextMock.Object, tokenHelperMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.Username.Should().Be("newUsername"); // Check if username is updated
        user.Mail.Should().Be("new@mail.com"); // Check if mail is updated
        contextMock.Verify(c => c.SaveToDbAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}