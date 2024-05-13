using System.Net;
using Common.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Common.AspNetCore;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
public abstract class BaseApiController : ControllerBase
{
    private RubiconmpProblemDetailsFactory _rubiconmpProblemDetailsFactory;

    public RubiconmpProblemDetailsFactory CyberCubeProblemDetailsFactory
    {
        get =>
            _rubiconmpProblemDetailsFactory ??= HttpContext?.RequestServices?.GetRequiredService<RubiconmpProblemDetailsFactory>();
        set => _rubiconmpProblemDetailsFactory = value ?? throw new ArgumentNullException(nameof(value));
    }


    [NonAction]
    public virtual ActionResult BadRequest([ActionResultObjectValue] params string[] errors) =>
        BadRequest(detail: null, errors: errors);

    [NonAction]
    public virtual ActionResult BadRequest(
        string detail = null,
        string instance = null,
        int? statusCode = null,
        string title = null,
        string type = null,
        [ActionResultObjectValue] params string[] errors)
    {
        var errorsList = new List<string>(errors);

        var errorProblem = CyberCubeProblemDetailsFactory.CreateProblemDetailsWithErrors(
            HttpContext,
            errorsList,
            statusCode,
            title,
            type,
            detail,
            instance);

        if (errorProblem.Status == 400)
        {
            // For compatibility with 2.x, continue producing BadRequestObjectResult instances if the status code is 400.
            return new BadRequestObjectResult(errorProblem);
        }

        return new ObjectResult(errorProblem)
        {
            StatusCode = errorProblem.Status
        };
    }
}