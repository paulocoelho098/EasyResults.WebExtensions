using EasyResults.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace EasyResults.WebExtensions
{
    /// <summary>
    /// Result handler class for Web purposes
    /// </summary>
    /// <typeparam name="T">Type of the data returned by Result</typeparam>
    public class ResultHandler<T> : ResultHandler<T, ActionResult>
    {
    }
}
