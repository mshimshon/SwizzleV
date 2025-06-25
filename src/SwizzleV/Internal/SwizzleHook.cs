namespace SwizzleV.Internal;
internal class SwizzleHook : ISwizzleHook
{
    private readonly WeakReference<object> _instance;
    private readonly WeakReference<object> _viewModel;
    private readonly WeakReference<Func<Task>> _listener;

    public WeakReference<object> Instance => _instance;
    public WeakReference<object> ViewModel => _viewModel;
    public WeakReference<Func<Task>> Listener => _listener;

    public object? GetInstance => Instance.TryGetTarget(out var target) ? target : default;

    public object? GetViewModel => ViewModel.TryGetTarget(out var target) ? target : default;

    public Func<Task>? GetListener => Listener.TryGetTarget(out var target) ? target : default;

    public SwizzleHook(object instance, object viewModel, Func<Task> listener)
    {
        _listener = new WeakReference<Func<Task>>(listener);
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
}
