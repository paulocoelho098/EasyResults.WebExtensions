﻿using EasyResults.Enums;
using EasyResults.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace EasyResults.WebExtensions
{
    /// <summary>
    /// Class to add specific behaviours to ResultHandler to be used in Web applications
    /// </summary>
    public static class ResultHandlerWebExtensions
    {
        /// <summary>
        /// Extension method that will return a Problem when result has a status of client error
        /// </summary>
        /// <typeparam name="T">Type of the data returned by Result</typeparam>
        /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
        /// <param name="controller">Web controller</param>
        /// <returns>A ResultHandler with ActionResult as the type of return</returns>
        public static ResultHandler<T, ActionResult> UseDefaultWebClientErrorHandler<T>(this ResultHandler<T, ActionResult> executionHandler, ControllerBase controller)
        {
            executionHandler.OnClientError((serviceResult) =>
            {
                return controller.Problem(statusCode: MapToIntHttpStatusCode(serviceResult.Status),
                                          title: serviceResult.Message);
            });

            return executionHandler;
        }

        /// <summary>
        /// Extension method that will return a Problem when result has a status of server error
        /// </summary>
        /// <typeparam name="T">Type of the data returned by Result</typeparam>
        /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
        /// <param name="controller">Web controller</param>
        /// <returns>A ResultHandler with ActionResult as the type of return</returns>
        public static ResultHandler<T, ActionResult> UseDefaultWebServerErrorHandler<T>(this ResultHandler<T, ActionResult> executionHandler, ControllerBase controller)
        {
            executionHandler.OnServerError((serviceResult) =>
            {
                return controller.Problem(statusCode: MapToIntHttpStatusCode(serviceResult.Status),
                                          title: serviceResult.Message);
            });

            return executionHandler;
        }

        /// <summary>
        /// Extension method that will return a Problem when result has a status of an error
        /// </summary>
        /// <typeparam name="T">Type of the data returned by Result</typeparam>
        /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
        /// <param name="controller">Web controller</param>
        /// <returns>A ResultHandler with ActionResult as the type of return</returns>
        public static ResultHandler<T, ActionResult> UseWebDefaultErrorHandler<T>(this ResultHandler<T, ActionResult> executionHandler, ControllerBase controller)
        {
            UseDefaultWebClientErrorHandler(executionHandler, controller);
            UseDefaultWebServerErrorHandler(executionHandler, controller);

            return executionHandler;
        }

        /// <summary>
        /// Extension method that will return the same status as the result when no other handler is hitted
        /// </summary>
        /// <typeparam name="T">Type of the data returned by Result</typeparam>
        /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
        /// <returns>A ResultHandler with ActionResult as the type of return</returns>
        public static ResultHandler<T, ActionResult> UseWebDefaultUnhittedHandler<T>(this ResultHandler<T, ActionResult> executionHandler)
        {
            executionHandler.OnUnhittedHandler((serviceResult) =>
            {
                return new StatusCodeResult(MapToIntHttpStatusCode(serviceResult.Status));
            });

            return executionHandler;
        }

        /// <summary>
        /// Extension method that will add all supported default web behaviors
        /// </summary>
        /// <typeparam name="T">Type of the data returned by Result</typeparam>
        /// <param name="executionHandler">The ResultHandler instance to which this extension method will be applied.</param>
        /// <param name="controller">Web controller</param>
        /// <returns>A ResultHandler with ActionResult as the type of return</returns>
        public static ResultHandler<T, ActionResult> UseWebDefaultsHandler<T>(this ResultHandler<T, ActionResult> executionHandler, ControllerBase controller)
        {
            UseWebDefaultErrorHandler(executionHandler, controller);
            UseWebDefaultUnhittedHandler(executionHandler);

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
}
