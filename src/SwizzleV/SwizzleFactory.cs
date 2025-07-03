using Microsoft.Extensions.DependencyInjection;
using SwizzleV.Internal;
using System.Collections.Concurrent;

namespace SwizzleV;
internal class SwizzleFactory : ISwizzleFactory, ISwizzleViewModel
{
    private readonly IServiceProvider _serviceProvider;
    protected ConcurrentDictionary<SwizzleHook, bool> _cache = new();
    public SwizzleFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public virtual ISwizzleHook CreateOrGet<TViewModel>(Func<object> key, Func<Task> onUpdate) where TViewModel : class
    {
        var obj = key();
        if (TryGetByCaller<TViewModel>(obj, out var existingHook))
            return existingHook!;
        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        var hook = new SwizzleHook(key(), vm, onUpdate);
        _cache.TryAdd(hook, true);
        return hook;
    }

    public Task SpreadChanges(Func<object> viewModel)
    {
        var vmInput = viewModel();
        foreach (var item in _cache.Keys)
        {
            bool hasInstance = item.Instance.TryGetTarget(out var target);
            bool hasViewModel = item.ViewModel.TryGetTarget(out var vm);

            if (!hasInstance || !hasViewModel)
            {
                _cache.TryRemove(item, out var _);
                continue;
            }
            if (ReferenceEquals(vm, vmInput))
            {
                item.GetListener(target!);
            }
        }
        return Task.CompletedTask;
    }

    public bool TryGetByCaller<TViewModel>(object value, out SwizzleHook? swizzleHook) where TViewModel : class
    {
        swizzleHook = default;
        foreach (var item in _cache.Keys)
        {

            if (item.Instance.TryGetTarget(out var targetInstance) &&
                item.ViewModel.TryGetTarget(out var targetVm))
            {
                if (ReferenceEquals(value, targetInstance) && targetVm.GetType() == typeof(TViewModel))
                {
                    swizzleHook = item;
                    return true;
                }
            }
            else
            {
                _cache.TryRemove(item, out var _);
            }
        }
        return false;
    }

}
