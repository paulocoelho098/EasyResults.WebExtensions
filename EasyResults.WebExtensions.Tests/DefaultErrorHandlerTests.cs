using EasyResults.Entities;
using EasyResults.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EasyResults.WebExtensions.Tests;

public class DefaultErrorHandlerTests
{
    [Theory]
    [InlineData(Status.BadRequest, "BadRequest", 400)]
    [InlineData(Status.Unauthorized, "Unauthorized", 401)]
    [InlineData(Status.Forbidden, "Forbidden", 403)]
    [InlineData(Status.NotFound, "NotFound", 404)]
    [InlineData(Status.Conflict, "Conflict", 409)]
    [InlineData(Status.InternalServerError, "InternalServerError", 500)]
    public void UseWebDefaultErrorHandler_ReturnsCorrectProblem_ForDifferentErrorStatusCodes(Status statusCode, string expectedTitle, int httpStatusCode)
    {
        // Arrange
        var resultHandler = new ResultHandler()
            .UseWebDefaultErrorHandler(new TestController());

        // Act
        ActionResult result = resultHandler.HandleResult(new Result<string>(statusCode, expectedTitle));

        // Assert
        ObjectResult problem = Assert.IsType<ObjectResult>(result);
        Assert.Equal(httpStatusCode, problem.StatusCode);
        ProblemDetails problemDetails = Assert.IsType<ProblemDetails>(problem.Value);
        Assert.Equal(expectedTitle, problemDetails.Title);
    }
}
