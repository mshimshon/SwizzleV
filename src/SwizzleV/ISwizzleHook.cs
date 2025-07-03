namespace SwizzleV;
public interface ISwizzleHook
{
    TParent? GetInstance<TParent>() where TParent : class;
    TViewModel? GetViewModel<TViewModel>() where TViewModel : class;
    Func<object, Task> GetListener { get; }
}
