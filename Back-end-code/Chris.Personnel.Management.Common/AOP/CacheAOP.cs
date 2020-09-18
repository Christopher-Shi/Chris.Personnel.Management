using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Chris.Personnel.Management.Common.Attributes;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Common.Helper;

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
                // //获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);
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

        #region private method
        /// <summary>
        /// 自定义缓存的key
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key = $"{key}{param}:";
            }

            return key.TrimEnd(':');
        }

        /// <summary>
        /// object 转 string
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static string GetArgumentValue(object arg)
        {
            if (arg is DateTime || arg is DateTime?)
            {
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");
            }

            if (arg is string || arg is ValueType || arg is Nullable)
            {
                return arg.ToString();
            }

            if (arg != null)
            {
                if (arg is Expression obj)
                {
                    var result = Resolve(obj);
                    return Common.Helper.MD5Helper.MD5Encrypt16(result);
                }

                if (arg.GetType().IsClass)
                {
                    return MD5Helper.MD5Encrypt16(Newtonsoft.Json.JsonConvert.SerializeObject(arg));
                }
            }
            return string.Empty;
        }

        private static string Resolve(Expression expression)
        {
            if (expression is LambdaExpression)
            {
                LambdaExpression lambda = expression as LambdaExpression;
                expression = lambda.Body;
                return Resolve(expression);
            }
            if (expression is BinaryExpression binary)
            {
                if (binary.Left is MemberExpression && binary.Right is ConstantExpression)//解析x=>x.Name=="123" x.Age==123这类
                    return ResolveFunc(binary.Left, binary.Right, binary.NodeType);
                if (binary.Left is MethodCallExpression && binary.Right is ConstantExpression)//解析x=>x.Name.Contains("xxx")==false这类的
                {
                    object value = (binary.Right as ConstantExpression).Value;
                    return ResolveLinqToObject(binary.Left, value, binary.NodeType);
                }
                if ((binary.Left is MemberExpression && binary.Right is MemberExpression)
                    || (binary.Left is MemberExpression && binary.Right is UnaryExpression))//解析x=>x.Date==DateTime.Now这种
                {
                    LambdaExpression lambda = Expression.Lambda(binary.Right);
                    Delegate fn = lambda.Compile();
                    ConstantExpression value = Expression.Constant(fn.DynamicInvoke(null), binary.Right.Type);
                    return ResolveFunc(binary.Left, value, binary.NodeType);
                }
            }
            if (expression is UnaryExpression unary)
            {
                if (unary.Operand is MethodCallExpression)//解析!x=>x.Name.Contains("xxx")或!array.Contains(x.Name)这类
                    return ResolveLinqToObject(unary.Operand, false);
                if (unary.Operand is MemberExpression && unary.NodeType == ExpressionType.Not)//解析x=>!x.isDeletion这样的 
                {
                    ConstantExpression constant = Expression.Constant(false);
                    return ResolveFunc(unary.Operand, constant, ExpressionType.Equal);
                }
            }
            if (expression is MemberExpression member && member.NodeType == ExpressionType.MemberAccess)//解析x=>x.isDeletion这样的 
            {
                ConstantExpression constant = Expression.Constant(true);
                return ResolveFunc(member, constant, ExpressionType.Equal);
            }
            if (expression is MethodCallExpression methodCall)//x=>x.Name.Contains("xxx")或array.Contains(x.Name)这类
            {
                return ResolveLinqToObject(methodCall, true);
            }
            var body = expression as BinaryExpression;
            //已经修改过代码body应该不会是null值了
            if (body == null)
                return string.Empty;
            var Operator = GetOperator(body.NodeType);
            var Left = Resolve(body.Left);
            var Right = Resolve(body.Right);
            string Result = string.Format("({0} {1} {2})", Left, Operator, Right);
            return Result;
        }

        private static string GetOperator(ExpressionType expressiontype)
        {
            switch (expressiontype)
            {
                case ExpressionType.And:
                    return "and";
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                    return "or";
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                default:
                    throw new Exception(string.Format("不支持{0}此种运算符查找！" + expressiontype));
            }
        }

        private static string ResolveFunc(Expression left, Expression right, ExpressionType expressiontype)
        {
            var name = (left as MemberExpression).Member.Name;
            var value = (right as ConstantExpression).Value;
            var Operator = GetOperator(expressiontype);
            return name + Operator + value ?? "null";
        }

        private static string ResolveLinqToObject(Expression expression, object value, ExpressionType? expressiontype = null)
        {
            var MethodCall = expression as MethodCallExpression;
            var MethodName = MethodCall.Method.Name;
            switch (MethodName)
            {
                case "Contains":
                    if (MethodCall.Object != null)
                        return Like(MethodCall);
                    return In(MethodCall, value);
                case "Count":
                    return Len(MethodCall, value, expressiontype.Value);
                case "LongCount":
                    return Len(MethodCall, value, expressiontype.Value);
                default:
                    throw new Exception(string.Format("不支持{0}方法的查找！", MethodName));
            }
        }

        private static string In(MethodCallExpression expression, object isTrue)
        {
            var Argument1 = (expression.Arguments[0] as MemberExpression).Expression as ConstantExpression;
            var Argument2 = expression.Arguments[1] as MemberExpression;
            var Field_Array = Argument1.Value.GetType().GetFields().First();
            object[] Array = Field_Array.GetValue(Argument1.Value) as object[];
            List<string> SetInPara = new List<string>();
            for (int i = 0; i < Array.Length; i++)
            {
                string Name_para = "InParameter" + i;
                string Value = Array[i].ToString();
                SetInPara.Add(Value);
            }
            string Name = Argument2.Member.Name;
            string Operator = Convert.ToBoolean(isTrue) ? "in" : " not in";
            string CompName = string.Join(",", SetInPara);
            string Result = string.Format("{0} {1} ({2})", Name, Operator, CompName);
            return Result;
        }

        private static string Like(MethodCallExpression expression)
        {
            var Temp = expression.Arguments[0];
            LambdaExpression lambda = Expression.Lambda(Temp);
            Delegate fn = lambda.Compile();
            var tempValue = Expression.Constant(fn.DynamicInvoke(null), Temp.Type);
            string Value = string.Format("%{0}%", tempValue);
            string Name = (expression.Object as MemberExpression).Member.Name;
            string Result = string.Format("{0} like {1}", Name, Value);
            return Result;
        }

        private static string Len(MethodCallExpression expression, object value, ExpressionType expressiontype)
        {
            object Name = (expression.Arguments[0] as MemberExpression).Member.Name;
            string Operator = GetOperator(expressiontype);
            string Result = string.Format("len({0}){1}{2}", Name, Operator, value.ToString());
            return Result;
        }
        #endregion
    }
}