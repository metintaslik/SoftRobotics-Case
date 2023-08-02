using Microsoft.EntityFrameworkCore;
using softrobotics.auth.application.UserHandler.Command;
using softrobotics.auth.domain.Entity;
using softrobotics.auth.infrastructure.Persistance;
using softrobotics.shared.Common.Helpers;

namespace softrobotics.auth.test;

public class UserActivateCommandHandlerTests : IDisposable
{
    private readonly DbContextOptions<SoftRoboticsDbContext> options;

    public UserActivateCommandHandlerTests()
    {
        // In-memory database options for testing
        options = new DbContextOptionsBuilder<SoftRoboticsDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Seed the in-memory database with an existing user validate entity
        using var dbContext = new SoftRoboticsDbContext(options);
        dbContext.UserValidates.Add(new UserValidate
        {
            UserValidateId = 1,
            UserId = 1,
            HashUUID = "valid-uuid-hash",
            IsActive = true,
            Created = DateTime.Now.AddMinutes(-5), // To simulate a recently created entity
            User = new User
            {
                UserId = 1,
                Username = "john.doe",
                Password = "PQrUv3l2e78zbDmTb6C4G7RvDhJkD6zX", // Assuming the password is already hashed
                Mail = "john.doe@example.com"
                // Set other required properties here if needed
            }
        });
        dbContext.SaveChanges();
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldActivateUserAndReturnSuccessResult()
    {
        // Arrange
        var request = new UserActivateCommand
        {
            UserID = 1,
            UUID = "valid-uuid-hash"
        };

        using (var dbContext = new SoftRoboticsDbContext(options))
        {
            var createUserCommandHandler = new UserActivateCommandHandler(dbContext);

            // Act
            var result = await createUserCommandHandler.Handle(request, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Errors);
        }
    }

    public void Dispose()
    {
        // Clean up the in-memory database after each test
        using var dbContext = new SoftRoboticsDbContext(options);
        dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Handle_WhenInvalidRequest_ShouldReturnFailureResult()
    {
        // Arrange
        var dbContextOptions = new DbContextOptionsBuilder<SoftRoboticsDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var command = new UserActivateCommand
        {
            UserID = 1,
            UUID = Guid.NewGuid().ToString().EncodeSHA256() // Invalid UUID
        };

        var dbContext = new SoftRoboticsDbContext(dbContextOptions);
        var handler = new UserActivateCommandHandler(dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }
}