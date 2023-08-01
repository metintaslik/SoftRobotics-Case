using softrobotics.auth.application.Common.Model;
using System.Collections.Generic;
using Xunit;

namespace softrobotics.auth.test
{
    public class ResultTests
    {
        [Fact]
        public void Result_Success_Should_Return_Successful_Result()
        {
            // Act
            var result = Result.Success();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Errors);
        }

        [Fact]
        public void Result_Failure_Should_Return_Failed_Result_WithNoErrors()
        {
            // Act
            var result = Result.Failure();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Errors);
        }

        [Fact]
        public void Result_Failure_WithErrors_Should_Return_Failed_Result_WithErrors()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            var result = Result.Failure(errors);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(errors, result.Errors);
        }
    }

    public class ResultOfTTests
    {
        [Fact]
        public void ResultOfT_Success_Should_Return_Successful_Result_WithData()
        {
            // Arrange
            var data = 42;

            // Act
            var result = Result<int>.Success(data);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Errors);
            Assert.Equal(data, result.Data);
        }

        [Fact]
        public void ResultOfT_Failure_Should_Return_Failed_Result_WithNoData()
        {
            // Act
            var result = Result<int>.Failure(errors: Enumerable.Empty<string>());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Errors); // Check if Errors collection is not null
            Assert.Empty(result.Errors); // Check if Errors collection is empty
            Assert.Equal(default(int), result.Data); // Default value for int (0)
        }

        [Fact]
        public void ResultOfT_Failure_WithErrors_Should_Return_Failed_Result_WithErrors()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            var result = Result<int>.Failure(errors);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(errors, result.Errors);
            Assert.Equal(default(int), result.Data); // Default value for int (0)
        }
    }
}
