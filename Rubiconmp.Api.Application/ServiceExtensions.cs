using Microsoft.Extensions.DependencyInjection;
using Rubiconmp.Api.Application.Handlers;

namespace Rubiconmp.Api.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetRectanglesBySegmentHandler).Assembly));
        services.AddRepositoriesAndUnitOfWork();

        return services;
    }
}