using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Common.Helper;
using Newtonsoft.Json;

namespace Chris.Personnel.Management.Common.AOP
{
    public class LogAOP : IInterceptor
    {
        private readonly NLogHelper _logger;

        public LogAOP()
        {
            _logger = NLogHelper.Default;
        }

        public void Intercept(IInvocation invocation)
        {
            var currentUserName = "chris";

            //记录被拦截方法信息的日志信息
            var dataIntercept = "" +
                                $"【当前操作用户】：{currentUserName} \r\n" +
                                $"【当前执行方法】：{invocation.Method.Name} \r\n" +
                                $"【携带的参数有】：{string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            try
            {
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();

                if (invocation.Method.IsAsyncMethod())
                {
                    var type = invocation.Method.ReturnType;
                    var resultProperty = type.GetProperty("Result");
                    if (resultProperty != null)
                    {
                        dataIntercept +=
                            $"【执行完成结果】：{JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue))}";
                    }

                    Parallel.For(0, 1, e =>
                    {
                        _logger.Info(dataIntercept);
                    });
                }
                else
                {
                    dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");
                    Parallel.For(0, 1, e =>
                    {
                        _logger.Info(dataIntercept);
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
