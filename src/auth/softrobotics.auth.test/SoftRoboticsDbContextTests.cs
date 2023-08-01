using Microsoft.EntityFrameworkCore;
using Moq;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.infrastructure.Persistance;

namespace softrobotics.auth.test
{
    public class SoftRoboticsDbContextTests
    {
        [Fact]
        public async Task SaveToDbAsync_Should_Call_SaveChangesAsync_Method()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<SoftRoboticsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var mockDbContext = new Mock<SoftRoboticsDbContext>(dbContextOptions) { CallBase = true };
            mockDbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1); // Fake SaveChangesAsync method

            ISoftRoboticsDbContext dbContext = mockDbContext.Object;

            // Act
            var result = await dbContext.SaveToDbAsync();

            // Assert
            Assert.Equal(1, result);
            mockDbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}