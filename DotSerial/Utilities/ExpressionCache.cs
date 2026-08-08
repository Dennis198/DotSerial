using System.Collections.Concurrent;
using System.Linq.Expressions;

/// <summary>
/// Caches compiled expression trees for property getters, setters, and method invokers.
/// </summary>
internal static class ExpressionCache
{
    // https://github.com/JamesNK/Newtonsoft.Json/blob/4f73e74372445108d2c1bda37b36e6f5e43402e0/Src/Newtonsoft.Json/Utilities/DynamicReflectionDelegateFactory.cs#L307
    private static readonly ConcurrentDictionary<string, Func<object, object>> GetterCache = new();
    private static readonly ConcurrentDictionary<string, Action<object?, object?>> MethodCache = new();
    private static readonly ConcurrentDictionary<string, Action<object?, object?>> SetterCache = new();

    /// <summary>
    /// Gets or creates a cached getter for the specified property.
    /// </summary>
    /// <param name="type">The type that contains the property.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>A function that gets the property value from an instance of the type.</returns>
    [Obsolete("Use Property.GetValue instead. Its currently faster than the compiled expression tree.")]
    internal static Func<object, object> GetOrCreateGetter(Type type, string propertyName)
    {
        var key = $"{type.FullName}_{propertyName}";

        return GetterCache.GetOrAdd(
            key,
            _ =>
            {
                var param = Expression.Parameter(typeof(object), "obj");
                var propertyInfo =
                    type.GetProperty(propertyName)
                    ?? throw new InvalidOperationException(
                        $"Property '{propertyName}' not found on '{type.FullName}'."
                    );
                // Static properties have no instance; use null expression target
                Expression propertyExpr =
                    propertyInfo.GetGetMethod()?.IsStatic == true
                        ? Expression.Property(null, propertyInfo)
                        : Expression.Property(Expression.Convert(param, type), propertyInfo);
                var castResult = Expression.Convert(propertyExpr, typeof(object));

                return Expression.Lambda<Func<object, object>>(castResult, param).Compile();
            }
        );
    }

    /// <summary>
    /// Gets or creates a cached method invoker for the specified method.
    /// </summary>
    /// <param name="type">The type that contains the method.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>An action that invokes the method on an instance of the type with the specified argument.</returns>
    internal static Action<object?, object?> GetOrCreateMethodInvoker(Type type, string methodName)
    {
        var key = $"{type.FullName}_{methodName}";
        return MethodCache.GetOrAdd(
            key,
            _ =>
            {
                var methodInfo =
                    type.GetMethod(methodName)
                    ?? throw new InvalidOperationException($"Method '{methodName}' not found on '{type.FullName}'.");
                var instanceParam = Expression.Parameter(typeof(object), "instance");
                var argParam = Expression.Parameter(typeof(object), "arg");
                var castInstance = Expression.Convert(instanceParam, type);
                var castArg = Expression.Convert(argParam, methodInfo.GetParameters()[0].ParameterType);
                var call = Expression.Call(castInstance, methodInfo, castArg);
                return Expression.Lambda<Action<object?, object?>>(call, instanceParam, argParam).Compile();
            }
        );
    }

    /// <summary>
    /// Gets or creates a cached setter for the specified property.
    /// </summary>
    /// <param name="type">The type that contains the property.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>An action that sets the property value on an instance of the type.</returns>
    [Obsolete("Use Property.SetValue instead. Its currently faster than the compiled expression tree.")]
    internal static Action<object?, object?> GetOrCreateSetter(Type type, string propertyName)
    {
        var key = $"{type.FullName}_{propertyName}";
        return SetterCache.GetOrAdd(
            key,
            _ =>
            {
                var propertyInfo =
                    type.GetProperty(propertyName)
                    ?? throw new InvalidOperationException(
                        $"Property '{propertyName}' not found on '{type.FullName}'."
                    );
                // Boxed value types can't be mutated via expression trees; fall back to PropertyInfo.SetValue
                if (type.IsValueType)
                    return (instance, value) => propertyInfo.SetValue(instance, value);
                var instanceParam = Expression.Parameter(typeof(object), "instance");
                var valueParam = Expression.Parameter(typeof(object), "value");
                var castValue = Expression.Convert(valueParam, propertyInfo.PropertyType);
                // Static properties have no instance; use null expression target
                Expression propertyTarget =
                    propertyInfo.GetSetMethod()?.IsStatic == true
                        ? Expression.Property(null, propertyInfo)
                        : Expression.Property(Expression.Convert(instanceParam, type), propertyInfo);
                var body = Expression.Assign(propertyTarget, castValue);
                return Expression.Lambda<Action<object?, object?>>(body, instanceParam, valueParam).Compile();
            }
        );
    }
}
