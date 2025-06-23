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

1. You register the ModelView as Transient for the isolated components not the pages.

```csharp
services.AddTransient<ComponentViewModel>();
```
2. Request is on the fly, you can call ```SwizzleFactory.GetViewModel<ComponentViewModel>(this);``` any where as many time safely... code below is for conviencien
```csharp
// Component.razor.cs
[Inject] private ComponentViewModel ViewModel => SwizzleFactory.GetViewModel<ComponentViewModel>(this);
```
