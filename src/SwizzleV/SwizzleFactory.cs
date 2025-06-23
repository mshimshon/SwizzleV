using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace SwizzleV;
internal class SwizzleFactory : ISwizzleFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConditionalWeakTable<object, object> _cache = new();

    public SwizzleFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TViewModel GetViewModel<TViewModel>(object key) where TViewModel : class
    {
        if (_cache.TryGetValue(key, out var existingVm))
        {
            return (TViewModel)existingVm;
        }

        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        _cache.Add(key, vm);
        return vm;
    }
}
