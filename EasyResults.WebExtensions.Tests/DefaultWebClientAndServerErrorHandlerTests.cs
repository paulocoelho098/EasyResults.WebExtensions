using EasyResults.Entities;
using EasyResults.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EasyResults.WebExtensions.Tests;


public class DefaultWebClientAndServerErrorHandlerTests
{
    [Theory]
    [InlineData(Status.BadRequest, "BadRequest", 400)]
    [InlineData(Status.Unauthorized, "Unauthorized", 401)]
    [InlineData(Status.Forbidden, "Forbidden", 403)]
    [InlineData(Status.NotFound, "NotFound", 404)]
    [InlineData(Status.Conflict, "Conflict", 409)]
    public void UseDefaultWebClientErrorHandler_ReturnsCorrectProblem_ForDifferentErrorStatusCodes(Status statusCode, string expectedTitle, int httpStatusCode)
    {
        // Arrange
        var resultHandler = new ResultHandler()
        .UseDefaultWebClientErrorHandler(new TestController());

        // Act
        ActionResult result = resultHandler.HandleResult(new Result<string>(statusCode, expectedTitle));

        // Assert
        ObjectResult problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(httpStatusCode, problem.StatusCode);
        ProblemDetails problemDetails = Assert.IsType<ProblemDetails>(problem.Value);
        Assert.Equal(expectedTitle, problemDetails.Title);
    }

    [Fact]
    public void UseDefaultWebServerErrorHandler_ReturnsCorrectProblem_ForInternalServerError()
    {
        // Arrange
        var resultHandler = new ResultHandler()
        .UseDefaultWebServerErrorHandler(new TestController());

        // Act
        ActionResult result = resultHandler.HandleResult(new Result<string>(Status.InternalServerError, "InternalServerError"));

        // Assert
        ObjectResult problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problem.StatusCode);
        ProblemDetails problemDetails = Assert.IsType<ProblemDetails>(problem.Value);
        Assert.Equal("InternalServerError", problemDetails.Title);
    }
}
