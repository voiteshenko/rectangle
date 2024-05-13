using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure;

public class RubiconmpProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options;

    public RubiconmpProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public ProblemDetails CreateProblemDetailsWithErrors(
        HttpContext httpContext,
        IList<string> errors,
        int? statusCode = null,
        string title = null,
        string type = null,
        string detail = null,
        string instance = null)
    {
        if (errors == null)
        {
            throw new ArgumentNullException(nameof(errors));
        }

        statusCode ??= 400;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        problemDetails.Extensions["businessErrors"] = errors;

        if (title != null)
        {
            // For validation problem details, don't overwrite the default title with null.
            problemDetails.Title = title;
        }

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }
    }
}