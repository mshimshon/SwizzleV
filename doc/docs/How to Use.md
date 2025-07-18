---
slug: how-to-use
title: How to Use
tags: [blazor, viewmodel, dependency-injection, swizzlev, component-patterns, scoped, transient, .net, csharp]
sidebar_position: 3
---

## Convention Related to View Models

ViewModels are traditionally registered as singletons or created per component when reusability is needed.  
This approach doesn't work well with **SwizzleV**, as it breaks the simplicity of dependency injection.  
SwizzleV is designed with the goal of using DI to resolve ViewModels cleanly and efficiently.

There are a few core conventions to follow (and we assume you're already familiar with the basics):

- **Do not push down properties that the ViewModel does not use.** This breaks separation of concerns.
- **Always rely on the ViewModel's properties first.** Use component-level properties only when something isn't available in the ViewModel.
  
This ensures consistency, maintainability, and clean separation between logic and rendering.

## Scenario: Global View Model

A **Global ViewModel** is used in pages or components that are **not reusable**.  
It holds the **UI state of the component**, and this state is **shared across all components of the same type**.

This is useful for top-level components like pages, layouts, or static content blocks that shouldn't reset state on reuse.

```csharp title="Articles.razor.cs"
using Microsoft.AspNetCore.Components;
using SwizzleV;
using System.Threading.Tasks;
using System.Collections.Generic;

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

```csharp title="ArticlesViewModel.cs"
using Microsoft.AspNetCore.Components;
using SwizzleV;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ArticlesViewModel
{
    public List<string> ArticleIds { get; set; } = new();
    public List<string> Articles { get; private set; } = new();
    private bool _loading = false;
    public bool Loading
    {
        get => _loading;
        set
        {
            if (value != _loading)
                _= _swizzleViewModel.SpreadChanges(() => this);
            _loading = value;
        }
    }
    private readonly ISwizzleViewModel _swizzleViewModel;

    public ArticleViewModel(ISwizzleViewModel swizzleViewModel)
    {
        _swizzleViewModel = swizzleViewModel;
    }

    public async Task LoadAsync()
    {
        Loading = true;
        await Task.Delay(500); // Simulate async load
        Articles = new List<string> { "a1", "a2", "a3" };
        Loading = false;
    }
}

```
```csharp title="Program.cs"
services.AddScoped<ArticlesViewModel>();
```

## Scenario: Per-Component View Model

**Per-Component ViewModels** are transient and scoped to each rendered instance of a component.  
This ensures that **every component has its own isolated state**, making them ideal for reusable components.

SwizzleV resolves these ViewModels using **dependency injection**, so you don't need to manually pass services to them.  
This keeps the architecture clean and consistent—constructor-injected services are automatically wired up, and the ViewModel is managed safely for the lifetime of the component instance.

The logic behind **Per-Component ViewModels** is exactly the same as for **Global ViewModels**.  
The only difference lies in how the ViewModel is registered:

- **Global ViewModels** are registered as `Scoped`
- **Per-Component ViewModels** are registered as `Transient`

This design makes it easy to switch between global and per-component behavior with **minimal migration effort**.  
SwizzleV handles the rest, maintaining clean dependency injection and lifecycle consistency.


```csharp title="ArticleCard.razor.cs"
using Microsoft.AspNetCore.Components;
using SwizzleV;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ArticleCard : ComponentBase
{
    [Inject] public ISwizzleVFactory SwizzleFactory { get; set; } = default!;

    private ArticleCardViewModel _viewModel = default!;
    [Parameter] public string ArticleId { get; set; } = default!;
    protected override async Task OnInitializedAsync()
    {
        // Create or Get Exisintg Hook Binding
        var articleVMHook = SwizzleFactory.CreateOrGet<ArticleCardViewModel>(() => this, () => InvokeAsync(() => StateHasChanged()));
        // Get View Model Type Instance of the Hook
        VM = articleVMHook.GetViewModel<ArticleCardViewModel>()!;
        // Push Down Paramters Used by View Model
        _viewModel.Id = ArticleId;
        await _viewModel.LoadAsync();
    }
}

```

```csharp title="ArticleCardViewModel.cs"
using Microsoft.AspNetCore.Components;
using SwizzleV;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ArticleCardViewModel
{
    public string ArticleId { get; set; } = default!;
    public string ArticleName { get; private set; }  = default!;
    public string ArticlePicture { get; private set; }  = default!;
    private bool _loading = false;
    public bool Loading
    {
        get => _loading;
        set
        {
            if (value != _loading)
                _= _swizzleViewModel.SpreadChanges(() => this);
            _loading = value;
        }
    }
    private readonly ISwizzleViewModel _swizzleViewModel;

    public ArticleViewModel(ISwizzleViewModel swizzleViewModel)
    {
        _swizzleViewModel = swizzleViewModel;
    }

    public async Task LoadAsync()
    {
        Loading = true;
        await Task.Delay(500); // Simulate async load
        ArticleName = "Maksim Shimshon";
        ArticlePicture = "https://funnybunny.gman/too-good-to-be-shown.jpg";
        Loading = false;
    }
}

```
```csharp title="Program.cs"
services.AddTransient<ArticleCardViewModel>();
```