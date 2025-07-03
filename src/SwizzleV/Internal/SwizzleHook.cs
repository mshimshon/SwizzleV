using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace SwizzleV.Internal;
internal class SwizzleHook : ISwizzleHook
{
    private readonly WeakReference<object> _instance;
    private readonly WeakReference<object> _viewModel;

    private readonly Func<object, Task> _compiledListener = default!;
    private readonly MethodInfo _methodListener = default!;

    public WeakReference<object> Instance => _instance;
    public WeakReference<object> ViewModel => _viewModel;
    public object? GetInstance => Instance.TryGetTarget(out var target) ? target : default;

    public object? GetViewModel => ViewModel.TryGetTarget(out var target) ? target : default;

    public Func<object, Task> GetListener => _compiledListener;

    public SwizzleHook(object instance, object viewModel, Func<Task> listener)
    {
        _methodListener = GetMethodInfoOrThrow(listener);
        _compiledListener = CreateDynamicInvoker(_methodListener);
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
    public static Func<object, Task> CreateDynamicInvoker(MethodInfo method)
    {
        var declaringType = method.DeclaringType ?? throw new ArgumentException("Method must have a declaring type");

        var dm = new DynamicMethod(
            $"__dyn_{method.Name}",
            typeof(Task),
            new[] { typeof(object) },
            declaringType.Module,
            skipVisibility: true);

        var il = dm.GetILGenerator();

        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Castclass, declaringType);

        il.EmitCall(OpCodes.Call, method, null);

        il.Emit(OpCodes.Ret);

        return (Func<object, Task>)dm.CreateDelegate(typeof(Func<object, Task>));
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
