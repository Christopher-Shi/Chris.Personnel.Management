using System.Reflection;
using Castle.DynamicProxy;
using Chris.Personnel.Management.Common.Attributes;
using Chris.Personnel.Management.Common.CommonService;
using Chris.Personnel.Management.Common.Extensions;

namespace Chris.Personnel.Management.Common.AOP
{
    public class MemoryCacheAOP : CacheAOPBase
    {
        private readonly IMemoryCaching _memoryCaching;

        public MemoryCacheAOP(IMemoryCaching memoryCaching)
        {
            _memoryCaching = memoryCaching;
        }

        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            // 对当前方法的特性判断
            if (method.GetCustomAttribute(typeof(CacheAttribute)) is CacheAttribute cacheAttribute)
            {
                // 获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);
                // 根据key获取相应的值
                var cacheValue = _memoryCaching.Get(cacheKey);
                if (cacheValue != null)
                {
                    // 将当前获取到的缓存值，赋值给当前执行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }

                // 去执行当前的方法
                invocation.Proceed();

                //存入缓存
                if (!cacheKey.IsNullOrEmpty())
                {
                    _memoryCaching.Set(cacheKey, invocation.ReturnValue, cacheAttribute.AbsoluteExpiration);
                }
            }
            else
            {
                invocation.Proceed(); // 直接执行被拦截方法
            }
        }
    }
}