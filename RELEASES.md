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
   
# v0.96
### Compatibility
- Added support for .NET Standard 2.0, Note: 2.0 will have degraded performances due to reflection.

# v0.95
- ViewModel caching using `WeakReference` to avoid redundant instances
- Simple `IViewModelFactory` interface for resolving and reusing ViewModels
- Scoped or Transient per-component lifetime support ideal for Blazor UI patterns
- Full support for constructor-based dependency injection
- Minimal configuration, register once with `AddSwizzleV()`
- Minimal external dependencies,  DI Abstraction.
- Clean architecture friendly, promotes separation of concerns
- Maximum Compatibility, supports .NET Standard 2.1