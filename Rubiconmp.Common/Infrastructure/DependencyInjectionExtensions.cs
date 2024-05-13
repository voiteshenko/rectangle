using Common.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRubiconmpProblemDetailsFactory(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddTransient<RubiconmpProblemDetailsFactory>();
        return services;
    }
}