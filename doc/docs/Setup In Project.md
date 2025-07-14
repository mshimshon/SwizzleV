---
slug: setup-in-project
title: Setup In Project
tags: [blazor, setup, installation, swizzlev, viewmodel-pattern, dependency-injection, scoped, transient, .net, csharp]
sidebar_position: 2
---


## 📦 Installation & Setup

```bash
Install-Package SwizzleV

dotnet add package SwizzleV

```

Add to ```Program.cs```: 
```csharp title="Program.cs"
builder.Services.AddSwizzleV();
```

Create SwzzleV common folder structure.
```
/Features/
/Features/Components/ComponentA.razor <- UI LEVEL
/Features/Components/ComponentA.razor.cs <- UI LEVEL @Code
/Features/Components/ViewModels/ComponentAViewModel.cs <- UI LEVEL Behavior no componan dependencies.
```

This structure is complete and common pattern.

## Add Services

### Registering ViewModels in SwizzleV

SwizzleV supports two types of ViewModel lifetimes based on component usage:

#### Transient ViewModels
- Used for reusable components where each instance requires its **own separate ViewModel state and behavior**.
- Instances are **not shared** across components.
- Ideal for components like cards, modals, or any UI fragment that needs isolated state.

```csharp title="Program.cs"
builder.Services.AddTransient<MyViewModel>();
```

#### Scoped ViewModels
- Used for non-reusable components such as Pages or the Current Logged Card.
- The ViewModel instance is **shared** across all components using the same ViewModel type within the scope.
- Useful for shared state or logic that multiple components need to access consistently.

```csharp title="Program.cs"
builder.Services.AddScoped<MyViewModel>();
```
