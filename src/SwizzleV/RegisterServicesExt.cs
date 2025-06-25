using Microsoft.Extensions.DependencyInjection;

namespace SwizzleV;
public static class RegisterServicesExt
{
    public static IServiceCollection AddSwizzleV(this IServiceCollection services)
    {
        // Register ViewModelFactory as singleton since it caches VMs
        services.AddScoped<ISwizzleFactory, SwizzleFactory>();
        services.AddScoped<ISwizzleViewModel, SwizzleFactory>();
        return services;
    }
}
