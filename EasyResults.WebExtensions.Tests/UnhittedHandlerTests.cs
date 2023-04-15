using EasyResults.Entities;
using EasyResults.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyResults.WebExtensions.Tests
{
    public class UnhittedHandlerTests
    {
        [Theory]
        [InlineData(Status.BadRequest, 400)]
        [InlineData(Status.Unauthorized, 401)]
        [InlineData(Status.Forbidden, 403)]
        [InlineData(Status.NotFound, 404)]
        [InlineData(Status.Conflict, 409)]
        [InlineData(Status.InternalServerError, 500)]
        public void UseWebDefaultUnhittedHandler_ReturnsCorrectProblem_ForDifferentErrorStatusCodes(Status statusCode, int httpStatusCode)
        {
            // Arrange
            var resultHandler = new ResultHandler<string>()
            .UseWebDefaultUnhittedHandler();
            // Act
            ActionResult result = resultHandler.HandleResult(new Result<string>(statusCode));

            // Assert
            StatusCodeResult problem = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(httpStatusCode, problem.StatusCode);
        }
    }
}
