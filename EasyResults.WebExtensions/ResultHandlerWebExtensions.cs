using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Handlers;
using EasyResults.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace EasyResults.WebExtensions;

/// <summary>
/// Class to add specific behaviours to ResultHandler to be used in Web applications
/// </summary>
public static class ResultHandlerWebExtensions
{
    /// <summary>
    /// Extension method that will return a Problem when result has a status of client error
    /// </summary>
    /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
    /// <param name="controller">Web controller</param>
    /// <param name="result">Result that is being handled</param>
    /// <param name="resultHandler">Lambda that allows you to handle the result</param>
    /// <returns>A ResultHandler with ActionResult as the type of return</returns>
    public static ResultHandler UseDefaultWebClientErrorHandler(this ResultHandler executionHandler, ControllerBase controller, IResult result, Action<ActionResult> resultHandler)
    {
        executionHandler.OnClientError(() =>
        {
            ActionResult output = controller.Problem(statusCode: MapToIntHttpStatusCode(result.Status),
                                        title: result.Message);
            resultHandler(output);
        });

        return executionHandler;
    }

    /// <summary>
    /// Extension method that will return a Problem when result has a status of server error
    /// </summary>
    /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
    /// <param name="controller">Web controller</param>
    /// <param name="result">Result that is being handled</param>
    /// <param name="resultHandler">Lambda that allows you to handle the result</param>
    /// <returns>A ResultHandler with ActionResult as the type of return</returns>
    public static ResultHandler UseDefaultWebServerErrorHandler(this ResultHandler executionHandler, ControllerBase controller, IResult result, Action<ActionResult> resultHandler)
    {
        executionHandler.OnServerError(() =>
        {
            ActionResult output = controller.Problem(statusCode: MapToIntHttpStatusCode(result.Status),
                                        title: result.Message);
            resultHandler(output);
        });

        return executionHandler;
    }

    /// <summary>
    /// Extension method that will return a Problem when result has a status of an error
    /// </summary>
    /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
    /// <param name="controller">Web controller</param>
    /// <param name="result">Result that is being handled</param>
    /// <param name="resultHandler">Lambda that allows you to handle the result</param>
    /// <returns>A ResultHandler with ActionResult as the type of return</returns>
    public static ResultHandler UseWebDefaultErrorHandler(this ResultHandler executionHandler, ControllerBase controller, IResult result, Action<ActionResult> resultHandler)
    {
        UseDefaultWebClientErrorHandler(executionHandler, controller, result, resultHandler);
        UseDefaultWebServerErrorHandler(executionHandler, controller, result, resultHandler);

        return executionHandler;
    }

    /// <summary>
    /// Extension method that will return the same status as the result when no other handler is hitted
    /// </summary>
    /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
    /// <param name="result">Result that is being handled</param>
    /// <param name="resultHandler">Lambda that allows you to handle the result</param>
    /// <returns>A ResultHandler with ActionResult as the type of return</returns>
    public static ResultHandler UseWebDefaultUnhittedHandler(this ResultHandler executionHandler, IResult result, Action<ActionResult> resultHandler)
    {
        executionHandler.OnUnhittedHandler(() =>
        {
            ActionResult output = new StatusCodeResult(MapToIntHttpStatusCode(result.Status));
            resultHandler(output);
        });

        return executionHandler;
    }

    /// <summary>
    /// Extension method that will add all supported default web behaviors
    /// </summary>
    /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
    /// <param name="controller">Web controller</param>
    /// <param name="resultHandler">Lambda that allows you to handle the result</param>
    /// <returns>A ResultHandler with ActionResult as the type of return</returns>
    public static ResultHandler UseWebDefaultsHandler(this ResultHandler executionHandler, ControllerBase controller, IResult result, Action<ActionResult> resultHandler)
    {
        UseWebDefaultErrorHandler(executionHandler, controller, result, resultHandler);
        UseWebDefaultUnhittedHandler(executionHandler, result, resultHandler);

        return executionHandler;
    }

    /// <summary>
    /// Map library status codes to http status codes
    /// </summary>
    /// <param name="status">Library status code</param>
    /// <returns>Http status code as int</returns>
    /// <exception cref="UnreachableException">If it receives an unknown library status code</exception>
    public static int MapToIntHttpStatusCode(Status status)
    {
        return status switch
        {
            Status.Success => (int)HttpStatusCode.OK,
            Status.BadRequest => (int)HttpStatusCode.BadRequest,
            Status.Unauthorized => (int)HttpStatusCode.Unauthorized,
            Status.Forbidden => (int)HttpStatusCode.Forbidden,
            Status.NotFound => (int)HttpStatusCode.NotFound,
            Status.Conflict => (int)HttpStatusCode.Conflict,
            Status.InternalServerError => (int)HttpStatusCode.InternalServerError,
            _ => throw new UnreachableException("MapToHttpStatusCode unknown status"),
        };
    }
}
