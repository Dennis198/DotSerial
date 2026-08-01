using System.Collections.Concurrent;
using System.Linq.Expressions;

public static class ExpressionCache
{
    //TODO string methoden als konstanten definieren, um typos zu vermeiden
    private static readonly ConcurrentDictionary<string, Func<object, object>> GetterCache = new();

    private static readonly ConcurrentDictionary<string, Action<object?, object?>> MethodCache = new();

    private static readonly ConcurrentDictionary<string, Action<object?, object?>> SetterCache = new();

    public static Func<object, object> GetOrCreateGetter(Type type, string propertyName)
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

    public static Action<object?, object?> GetOrCreateMethodInvoker(Type type, string methodName)
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

    public static Action<object?, object?> GetOrCreateSetter(Type type, string propertyName)
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
