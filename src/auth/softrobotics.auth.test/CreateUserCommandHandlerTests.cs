using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using softrobotics.auth.application.UserHandler.Command;
using softrobotics.auth.domain.Entity;
using softrobotics.auth.infrastructure.Persistance;

namespace softrobotics.auth.test;

public class CreateUserCommandHandlerTests : IDisposable
{
    private readonly DbContextOptions<SoftRoboticsDbContext> _options;
    private readonly SoftRoboticsDbContext _context;
    private readonly Mock<IMediator> mockMediator;

    public CreateUserCommandHandlerTests()
    {
        _options = new DbContextOptionsBuilder<SoftRoboticsDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new SoftRoboticsDbContext(_options);
        mockMediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldCreateUser()
    {
        // Arrange
        var createUserCommand = new CreateUserCommand
        {
            Username = "john.doe",
            Password = "PQrUv3l2e78zbDmTb6C4G7RvDhJkD6zX", // Assuming the password is already hashed
            Mail = "john.doe@example.com"
        };

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserCommand, User>();
        });
        var mapper = mapperConfig.CreateMapper();

        var createUserCommandHandler = new CreateUserCommandHandler(_context, mapper, mockMediator.Object);

        // Act
        var result = await createUserCommandHandler.Handle(createUserCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);

        // Verify that AddAsync and SaveToDbAsync methods were called
        Assert.Single(_context.Users); // Ensure only one user was added to the database
        var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == createUserCommand.Username);
        Assert.NotNull(createdUser); // Ensure the user with the specified username exists in the database
        Assert.Equal(createUserCommand.Mail, createdUser.Mail); // Ensure the email matches
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldReturnFailure()
    {
        // Arrange
        var createUserCommand = new CreateUserCommand
        {
            Username = "john.doe",
            Password = "PQrUv3l2e78zbDmTb6C4G7RvDhJkD6zX", // Assuming the password is already hashed
            Mail = "john.doe@example.com"
        };

        _context.Users.Add(new User
        {
            Username = "john.doe",
            Password = "PQrUv3l2e78zbDmTb6C4G7RvDhJkD6zX", // Assuming the password is already hashed
            Mail = "john.doe@example.com"
        });
        await _context.SaveChangesAsync();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserCommand, User>();
        });
        var mapper = mapperConfig.CreateMapper();

        var createUserCommandHandler = new CreateUserCommandHandler(_context, mapper, mockMediator.Object);

        // Act
        var result = await createUserCommandHandler.Handle(createUserCommand, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
    }

    // Other test methods...

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}