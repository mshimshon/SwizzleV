#if NETSTANDARD2_1_OR_GREATER
using System.Reflection;
using System.Reflection.Emit;
namespace SwizzleV.Internal;
internal partial class SwizzleHook
{


    public static Func<object, Task> CreateDynamicInvoker(MethodInfo method)
    {
        var declaringType = method.DeclaringType ?? throw new ArgumentException("Method must have a declaring type");

        var dm = new DynamicMethod(
            $"__dyn_{method.Name}",
            typeof(Task),
            new[] { typeof(object) },
            declaringType.Module,
            skipVisibility: true);

        var il = dm.GetILGenerator();

        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Castclass, declaringType);

        il.EmitCall(OpCodes.Call, method, null);

        il.Emit(OpCodes.Ret);

        return (Func<object, Task>)dm.CreateDelegate(typeof(Func<object, Task>));
    }
}
#endif