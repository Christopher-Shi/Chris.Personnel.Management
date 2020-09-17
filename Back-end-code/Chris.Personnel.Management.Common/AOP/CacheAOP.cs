using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Chris.Personnel.Management.Common.Attributes;
using Chris.Personnel.Management.Common.Extensions;

namespace Chris.Personnel.Management.Common.AOP
{
    public class CacheAOP : IInterceptor
    {
        private readonly IRedisCacheManager _redisCacheManager;

        public CacheAOP(IRedisCacheManager redisCacheManager)
        {
            _redisCacheManager = redisCacheManager;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
            {
                invocation.Proceed();
                return;
            }

            // 对当前方法的特性判断
            if (method.GetCustomAttribute(typeof(CacheAttribute)) is CacheAttribute cacheAttribute)
            {
                var cacheKey = "chris";//TODO:
                var cacheValue = _redisCacheManager.GetValue(cacheKey);
                if (cacheValue != null)
                {
                    var returnType = typeof(Task).IsAssignableFrom(method.ReturnType)
                        ? method.ReturnType.GenericTypeArguments.FirstOrDefault()
                        : method.ReturnType;

                    dynamic result = Json.FromJsonDynamic(returnType, cacheValue);
                    invocation.ReturnValue = typeof(Task).IsAssignableFrom(method.ReturnType)
                        ? Task.FromResult(result)
                        : result;
                }
                else
                {
                    invocation.Proceed();

                    // 存储缓存
                    if (!cacheKey.IsNullOrEmpty())
                    {
                        object response;

                        var type = invocation.Method.ReturnType;
                        if (typeof(Task).IsAssignableFrom(type))
                        {
                            var resultProperty = type.GetProperty("Result");
                            response = resultProperty.GetValue(invocation.ReturnValue);
                        }
                        else
                        {
                            response = invocation.ReturnValue;
                        }

                        if (response == null) response = string.Empty;
                        _redisCacheManager.Set(cacheKey, response, TimeSpan.FromMinutes(cacheAttribute.AbsoluteExpiration));
                    }
                }
            }
            else
            {
                invocation.Proceed();
            }
        }

        ///// <summary>
        ///// 自定义缓存的key
        ///// </summary>
        ///// <param name="invocation"></param>
        ///// <returns></returns>
        //private string CustomCacheKey(IInvocation invocation)
        //{
        //    var typeName = invocation.TargetType.Name;
        //    var methodName = invocation.Method.Name;
        //    var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

        //    string key = $"{typeName}:{methodName}:";
        //    foreach (var param in methodArguments)
        //    {
        //        key = $"{key}{param}:";
        //    }

        //    return key.TrimEnd(':');
        //}

        ///// <summary>
        ///// object 转 string
        ///// </summary>
        ///// <param name="arg"></param>
        ///// <returns></returns>
        //private static string GetArgumentValue(object arg)
        //{
        //    if (arg is DateTime || arg is DateTime?)
        //        return ((DateTime)arg).ToString("yyyyMMddHHmmss");

        //    if (arg is string || arg is ValueType || arg is Nullable)
        //        return arg.ToString();

        //    if (arg != null)
        //    {
        //        if (arg is Expression)
        //        {
        //            var obj = arg as Expression;
        //            var result = Resolve(obj);
        //            return Common.Helper.MD5Helper.MD5Encrypt16(result);
        //        }
        //        else if (arg.GetType().IsClass)
        //        {
        //            return Common.Helper.MD5Helper.MD5Encrypt16(Newtonsoft.Json.JsonConvert.SerializeObject(arg));
        //        }
        //    }
        //    return string.Empty;
        //}
    }
}