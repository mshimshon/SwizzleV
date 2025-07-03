using Microsoft.Extensions.DependencyInjection;

namespace SwizzleV;
public static class RegisterServicesExt
{
    public static IServiceCollection AddSwizzleV(this IServiceCollection services)
    {
        // Register ViewModelFactory as singleton since it caches VMs
        services.AddScoped<SwizzleFactory>();
        services.AddScoped<ISwizzleFactory>((p) => p.GetRequiredService<SwizzleFactory>());
        services.AddScoped<ISwizzleViewModel>((p) => p.GetRequiredService<SwizzleFactory>());
        return services;
    }
}
