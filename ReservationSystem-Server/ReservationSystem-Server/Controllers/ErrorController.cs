using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ReservationSystem_Server.Models;

namespace ReservationSystem_Server.Controllers;

public class ErrorController : Controller
{
    private readonly ICompositeViewEngine _viewEngine;
    
    public ErrorController(ICompositeViewEngine viewEngine)
    {
        _viewEngine = viewEngine;
    }
    
    // GET
    public IActionResult Index(int statusCode)
    {
        var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        if (feature == null)
        {
            // If the action is called directly, fudge a 404 error
            statusCode = StatusCodes.Status404NotFound;
            feature = new StatusCodeReExecuteFeature
            {
                OriginalPath = "/Error"
            };
        }

        Response.StatusCode = statusCode;
        
        // Return basic JSON in the event of API call
        if (feature.OriginalPath.StartsWith("/api"))
        {
            return Json(new
            {
                statusCode, 
                feature.OriginalPath
            });
        }

        StatusCodeErrorViewModel vm = new()
        {
            StatusCode = statusCode,
            ErrorMessage = ((HttpStatusCode)statusCode).ToString(),
            ErrorUrl = feature.OriginalPath
        };

        // Return the most specific view available, either a view named after a status code, or a generic status code view
        return _viewEngine.FindView(ControllerContext, statusCode.ToString(), false).Success 
            ? View(statusCode.ToString(), vm) 
            : View(vm);
    }
}