using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace EasyResults.WebExtensions.Tests;

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
        Result<string> resulthandling = new Result<string>(statusCode);
        ActionResult result = new ContentResult();

        ResultHandler resultHandler = new ResultHandler()
        .UseWebDefaultUnhittedHandler(resulthandling, (r) => {
            result = r;
        });
        // Act
        resultHandler.HandleResult(resulthandling);

        // Assert
        StatusCodeResult problem = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(httpStatusCode, problem.StatusCode);
    }
}

