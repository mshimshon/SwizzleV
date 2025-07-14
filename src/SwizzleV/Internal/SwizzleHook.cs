using System.Reflection;
using System.Runtime.CompilerServices;

namespace SwizzleV.Internal;
internal partial class SwizzleHook : ISwizzleHook
{
    private readonly WeakReference<object> _instance;
    private readonly WeakReference<object> _viewModel;
#if NETSTANDARD2_1_OR_GREATER

#endif
    private readonly MethodInfo _methodListener;
    private readonly Func<object, Task> _compiledListener;
    public Func<object, Task> GetListener => _compiledListener;
    public WeakReference<object> Instance => _instance;
    public WeakReference<object> ViewModel => _viewModel;
    public object? GetInstance => Instance.TryGetTarget(out var target) ? target : default;

    public object? GetViewModel => ViewModel.TryGetTarget(out var target) ? target : default;


    public SwizzleHook(object instance, object viewModel, Func<Task> listener)
    {
        _methodListener = GetMethodInfoOrThrow(listener);
#if NETSTANDARD2_1_OR_GREATER
        _compiledListener = CreateDynamicInvoker(_methodListener);
#else
        _compiledListener = async (instance) =>
        {
            await (Task)_methodListener.Invoke(instance, new object[] { })!;
        };
#endif
        _instance = new WeakReference<object>(instance);
        _viewModel = new WeakReference<object>(viewModel);
    }

    TParent? ISwizzleHook.GetInstance<TParent>() where TParent : class
    {
        Instance.TryGetTarget(out var target);
        return (TParent?)target;
    }
    TViewModel? ISwizzleHook.GetViewModel<TViewModel>() where TViewModel : class
    {
        _viewModel.TryGetTarget(out var target);
        return (TViewModel?)target;
    }
    public static MethodInfo GetMethodInfoOrThrow(Func<Task> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        MethodInfo method = func.Method;

        // Check for compiler-generated attribute, indicating lambda or anonymous method
        bool isCompilerGenerated = method.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true);

        if (isCompilerGenerated)
            throw new InvalidOperationException("Lambdas and anonymous methods are not allowed. Please pass a method group (e.g., instance.MethodName).");

        return method!;
    }
}
