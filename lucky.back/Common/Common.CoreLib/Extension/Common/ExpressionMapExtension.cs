using System.Reflection;
using System.Linq.Expressions;

namespace Common.CoreLib.Extension.Common
{
    /// <summary>
    /// 表达式映射扩展
    /// </summary>
    public static class SmartMappingExtensions
    {
        /// <summary>
        /// 自动构建实体到输出模型的映射表达式（支持基本类型转换）
        /// </summary>
        public static Expression<Func<TEntity, TOutput>> AutoMap<TEntity, TOutput>()
            where TEntity : class
            where TOutput : class, new()
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var bindings = new List<MemberBinding>();

            var entityProps = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var outputProp in typeof(TOutput).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!outputProp.CanWrite) continue;

                // 找同名属性（忽略大小写）
                var entityProp = entityProps.FirstOrDefault(p =>
                    p.Name.Equals(outputProp.Name, StringComparison.OrdinalIgnoreCase));

                if (entityProp != null)
                {
                    var propertyAccess = Expression.Property(parameter, entityProp);

                    // 如果类型不同但可转换，添加转换
                    if (entityProp.PropertyType != outputProp.PropertyType)
                    {
                        if (CanAutoConvert(entityProp.PropertyType, outputProp.PropertyType))
                        {
                            var converted = ConvertExpression(propertyAccess, outputProp.PropertyType);
                            bindings.Add(Expression.Bind(outputProp, converted));
                        }
                    }
                    else
                    {
                        bindings.Add(Expression.Bind(outputProp, propertyAccess));
                    }
                }
            }

            var memberInit = Expression.MemberInit(Expression.New(typeof(TOutput)), bindings);
            return Expression.Lambda<Func<TEntity, TOutput>>(memberInit, parameter);
        }

        private static bool CanAutoConvert(Type sourceType, Type targetType)
        {
            // 允许的自动转换：
            // 1. 可空 ↔ 不可空
            var underlyingTarget = Nullable.GetUnderlyingType(targetType);
            if (underlyingTarget != null && sourceType == underlyingTarget) return true;

            var underlyingSource = Nullable.GetUnderlyingType(sourceType);
            if (underlyingSource != null && underlyingSource == targetType) return true;

            // 2. 数值类型之间
            if (IsNumeric(sourceType) && IsNumeric(targetType)) return true;

            // 3. 转字符串
            if (targetType == typeof(string)) return true;

            return false;
        }

        private static bool IsNumeric(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(short) ||
                   type == typeof(byte) || type == typeof(double) || type == typeof(float) ||
                   type == typeof(decimal);
        }

        private static Expression ConvertExpression(Expression expr, Type targetType)
        {
            if (expr.Type == targetType) return expr;

            // 转字符串
            if (targetType == typeof(string))
            {
                return Expression.Call(expr, "ToString", null, null);
            }

            // 其他情况尝试 Convert
            return Expression.Convert(expr, targetType);
        }
    }

    /// <summary>
    /// 极简映射
    /// </summary>
    public static class UltraSimpleMapper
    {
        /// <summary>
        /// 自动构建实体到输出模型的映射表达式（支持基本类型转换）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public static Expression<Func<TEntity, TOutput>> AutoMap<TEntity, TOutput>()
            where TEntity : class
            where TOutput : class, new()
        {
            var param = Expression.Parameter(typeof(TEntity), "x");
            var entityProps = typeof(TEntity).GetProperties();
            var bindings = new List<MemberBinding>();

            foreach (var outputProp in typeof(TOutput).GetProperties())
            {
                var entityProp = entityProps.FirstOrDefault(p =>
                    p.Name.Equals(outputProp.Name, StringComparison.OrdinalIgnoreCase));

                if (entityProp != null && entityProp.PropertyType == outputProp.PropertyType)
                {
                    bindings.Add(Expression.Bind(outputProp, Expression.Property(param, entityProp)));
                }
            }

            return Expression.Lambda<Func<TEntity, TOutput>>(
                Expression.MemberInit(Expression.New(typeof(TOutput)), bindings),
                param);
        }
    }

    /// <summary>
    /// 极简映射扩展
    /// </summary>
    public static class SimpleMappingExtensions
    {
        /// <summary>
        /// 自动构建实体到输出模型的映射表达式（极简版）
        /// 只匹配同名属性（忽略大小写），类型兼容自动转换
        /// </summary>
        public static Expression<Func<TEntity, TOutput>> AutoMap<TEntity, TOutput>()
            where TEntity : class
            where TOutput : class, new()
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var bindings = new List<MemberBinding>();

            var entityProps = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var outputProp in typeof(TOutput).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!outputProp.CanWrite) continue;

                // 只找同名属性（忽略大小写）
                var entityProp = entityProps.FirstOrDefault(p =>
                    p.Name.Equals(outputProp.Name, StringComparison.OrdinalIgnoreCase));

                if (entityProp != null)
                {
                    var propertyAccess = Expression.Property(parameter, entityProp);
                    bindings.Add(Expression.Bind(outputProp, propertyAccess));
                }
            }

            var memberInit = Expression.MemberInit(Expression.New(typeof(TOutput)), bindings);
            return Expression.Lambda<Func<TEntity, TOutput>>(memberInit, parameter);
        }
    }
}
