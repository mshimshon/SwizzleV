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
