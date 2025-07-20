[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://opensource.org/licenses/MIT)
[![NuGet Version](https://img.shields.io/nuget/v/SwizzleV)](https://www.nuget.org/packages/SwizzleV)
[![](https://img.shields.io/nuget/dt/SwizzleV?label=Downloads)](https://www.nuget.org/packages/SwizzleV)



# SwizzleV

### [Official Documentation](https://swizzlev.com/)

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

1. Create a register your view model.
```csharp
public class Articles : ComponentBase
{
    [Inject] public ISwizzleVFactory SwizzleFactory { get; set; } = default!;

    private ArticlesViewModel _viewModel = default!;
    [Parameter] public List<string> ArticleIds { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        // Create or Get Exisintg Hook Binding
        var articleVMHook = SwizzleFactory
            .CreateOrGet<ArticlesViewModel>(() => this, ShouldUpdate);
        // Get View Model Type Instance of the Hook
        VM = articleVMHook.GetViewModel<ArticlesViewModel>()!;
        _viewModel.Id = ArticleIds;
        await _viewModel.LoadAsync();
    }
    private Task ShouldUpdate() => InvokeAsync(StateHasChanged);
}
```
```csharp
services.AddTransient<ArticleViewModel>();
// OR Scoped for non-reusable components like pages.
services.AddScoped<ArticleViewModel>();
```


```csharp
// Article.razor.cs
public partial class Article : ComponentBase
{
    [Parameter] public string Id { set; get; }
    [Inject] ISwizzleFactory SwizzleFactory { get; set; } = default!;
    private ArticleViewModel _articleViewModel = default!;

    protected override async Task OnInitializedAsync()
    {
        // Create or Get Exisintg Hook Binding
        var articleVMHook = SwizzleFactory.CreateOrGet<ArticleViewModel>(() => this, () => InvokeAsync(() => StateHasChanged()));
        // Get View Model Type Instance of the Hook
        _articleViewModel = articleVMHook.GetViewModel<ArticleViewModel>()!;
        // Push Down Paramters Used by View Model
        _articleViewModel.Id = Id;

    }
}
```


## 🔖 Versioning Policy

### 🚧 Pre-1.0.0 (`0.x.x`)

- The project is considered **Work In Progress**.
- **Breaking changes can occur at any time** without notice.
- No guarantees are made about stability or upgrade paths.

### ✅ Post-1.0.0 (`1.x.x` and beyond)

Follows a common-sense semantic versioning pattern:

- **Major (`X.0.0`)**  
  
  - Introduces major features or architectural changes  
  - May include well documented **breaking changes**

- **Minor (`1.X.0`)**  
  
  - Adds new features or enhancements  
  - May include significant bug fixes  
  - **No breaking changes**

- **Patch (`1.0.X`)**  
  
  - Hotfixes or urgent bug fixes  
  - Safe to upgrade  
  - **No breaking changes**
  