namespace SwizzleV;
public interface ISwizzleFactory
{
    TViewModel GetViewModel<TViewModel>(object key) where TViewModel : class;
}
