---
slug: gs-whatsswizzlev
title: What's SwizzleV
tags: [blazor, swizzlev, viewmodel-pattern, component-architecture, dependency-injection, scoped, transient, csharp, .net]
sidebar_position: 1
---

## What is SwizzleV?

SwizzleV is a lightweight framework for managing ViewModels and encapsulating component behavior in UI applications. It provides clear patterns for defining whether a ViewModel instance should be unique (transient) or shared (scoped), allowing developers to design components with well-contained logic and lifecycle control.

> SwizzleV is **not** a global state management system.  
It focuses on **local ViewModel instances**, giving developers precise control over behavior without introducing shared application-wide state.

## What Makes SwizzleV Different?

- **Precise Lifetime Control:** SwizzleV lets you define ViewModels as either transient (per component instance) or scoped (shared across related components). This prevents unintended reuse or leakage.

- **Framework-Agnostic Design:** ViewModels in SwizzleV are not tied to any specific UI framework. They work independently and can be used in Blazor, MAUI, WPF, or any other system that supports C#.

- **Explicit UI Binding via Interface:** Instead of automatic change propagation, SwizzleV uses a simple binding contract:  
  The **consumer** (such as a Blazor component) explicitly **binds to a ViewModel**, implementing an interface that exposes an `OnChange()` method or similar action.

- **Change Notification via `SpreadChanges()`:** When a ViewModel needs to notify its bound user (UI/s), it calls `SpreadChanges()`, which invokes the consumer’s provided callback. This enables full decoupling from UI frameworks and provides a clean reactive loop **without hardwiring** to component lifecycles.

## Summary

SwizzleV is purpose-built for managing **component-level behavior and logic** through ViewModels. Its framework-agnostic, opt-in change propagation system gives developers a high degree of flexibility, making it ideal for projects that value **modularity**, **clarity**, and **UI-framework independence**

