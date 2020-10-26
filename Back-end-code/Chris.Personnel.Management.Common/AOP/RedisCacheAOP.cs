using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Chris.Personnel.Management.Common.Attributes;
using Chris.Personnel.Management.Common.CommonService;
using Chris.Personnel.Management.Common.Extensions;

namespace Chris.Personnel.Management.Common.AOP
{
    public class RedisCacheAOP : CacheAOPBase
    {
        private readonly IRedisCacheManager _redisCacheManager;

        public RedisCacheAOP(IRedisCacheManager redisCacheManager)
        {
            _redisCacheManager = redisCacheManager;
        }

        public override void Intercept(IInvocation invocation)
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
                // //获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);
                var cacheValue = _redisCacheManager.GetValue(cacheKey);
                if (cacheValue != null)
                {
                    var returnType = typeof(Task).IsAssignableFrom(method.ReturnType)
                        ? method.ReturnType.GenericTypeArguments.FirstOrDefault()
                        : method.ReturnType;

                    var result = Json.FromJsonDynamic(returnType, cacheValue);
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

                        response ??= string.Empty; // if (response == null) response = string.Empty;

                        _redisCacheManager.Set(cacheKey, response, TimeSpan.FromMinutes(cacheAttribute.AbsoluteExpiration));
                    }
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}