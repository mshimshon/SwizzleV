# SwizzleV

**SwizzleV** is a lightweight and efficient ViewModel caching and factory library for Blazor and modern .NET applications.

It enables seamless management of transient ViewModel instances with built-in duplication and memory leaks prevention. 
SwizzleV combines the best of dependency injection, scoped lifetimes, and local component state without the common pitfalls of singletons.

**SwizzleV is an essential for any clean architecture blazor based front-end**.

## Features

- Transparent ViewModel caching with `WeakReference` to avoid redundant instances  
- Simple factory interface for resolving and reusing ViewModels  
- Ideal for Blazor components needing per-instance ViewModels  
- Full DI support with constructor injection  
- Minimal configuration, zero boilerplate  
- Improves component-based state management and lifecycle handling

## Installation

You can install SwizzleV via NuGet:

```bash
dotnet add package SwizzleV
```

## How to Use?

### Services

```csharp
services.AddSwizzleV();
```

Since the factory is a singleton registered, i recommend injecting it into the _Imports.cs.

```csharp
@using SwizzleV
@inject ISwizzleFactory SwizzleFactory
```
Then you register your View Models as you usually do "Scoped"... except the isolated view models components.
When a component need to maintain a separate state from each other you dont have many choice you either give up the View Model pattern or you instantiate the class view model into the lifecycle of the component...
but to me that is too much boiling plate when you work on large application so here how it is gonna go with SwizzleV.

1. Create a register your view model as you can see perfect pairing to StatePulse.Net.
```csharp
internal class ArticleViewModel
{
    private readonly IDispatcher _dispatcher;
    private readonly ISwizzleViewModel _swizzleViewModel;
    private readonly ArticleViewState _state;

    public bool IsLoading => _state.IsLoading;

    public ArticleViewModel(IStatePulse pulsars, ISwizzleViewModel swizzleViewModel, IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _swizzleViewModel = swizzleViewModel;
        _state = pulsars.StateOf<ArticleViewState>(this, OnStateHasChanged);
    }

    public async Task LoadAsync(string id)
    {
        var action = new ArticleGetOneAction(id);
        await _dispatcher.Prepare(() => action).DispatchAsync();
    }

    public Task OnStateHasChanged()
    {
        _swizzleViewModel.SpreadChanges(()=>this);
        return Task.CompletedTask;
    }
}
```
```csharp
services.AddTransient<ArticleViewModel>();
// OR Scoped for non-reusable components like pages.
services.AddScoped<ArticleViewModel>();
```

2. Create the components
```
@inherits ComponentBase
@page "/article/{Id}"
@if (_articleViewModel.IsLoading)
{
   <p>Loading...</p>
}
```



```csharp
// Article.razor.cs
public partial class Article : ComponentBase
{
    [Parameter] public string Id { set; get; }
    private ArticleViewModel _articleViewModel = default!;

    protected override async Task OnInitializedAsync()
    {
        var articleVMHook = SwizzleFactory
            .CreateOrGet<ArticleViewModel>(() => this, () => InvokeAsync(() => StateHasChanged()));
        _articleViewModel = articleVMHook.GetViewModel<ArticleViewModel>()!;
        await _articleViewModel.LoadAsync(Id);
    }
}
```
