namespace SwizzleV;
public interface ISwizzleViewModel
{
    Task SpreadChanges(Func<object> viewModel);
}
