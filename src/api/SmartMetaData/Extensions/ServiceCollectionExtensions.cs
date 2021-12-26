using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SmartMetaData.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ReplaceSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.Replace(ServiceDescriptor.Singleton<TService, TImplementation>());
        return services;
    }

    public static IServiceCollection ReplaceScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.Replace(ServiceDescriptor.Scoped<TService, TImplementation>());
        return services;
    }

    public static IServiceCollection ReplaceTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.Replace(ServiceDescriptor.Transient<TService, TImplementation>());
        return services;
    }
}
