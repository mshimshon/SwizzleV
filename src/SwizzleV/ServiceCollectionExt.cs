using Microsoft.Extensions.DependencyInjection;

namespace SwizzleV;
public static class ServiceCollectionExt
{
    public static IServiceCollection AddSwizzleV(this IServiceCollection services)
    {
        // Register ViewModelFactory as singleton since it caches VMs
        services.AddSingleton<ISwizzleFactory, SwizzleFactory>();
        return services;
    }
}
