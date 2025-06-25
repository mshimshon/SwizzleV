namespace SwizzleV;
public interface ISwizzleFactory
{
    ISwizzleHook CreateOrGet<TViewModel>(Func<object> key, Func<Task> onUpdate) where TViewModel : class;
}
