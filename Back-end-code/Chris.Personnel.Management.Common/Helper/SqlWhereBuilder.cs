using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Chris.Personnel.Management.Common.Helper
{
    /// <summary>
    /// 将 Expression<Func<T, bool>> expression 转换为 Sql Where 语句
    /// </summary>
    public class SqlWhereBuilder
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();
        public string ToSqlWhere<T>(Expression<Func<T, bool>> expression, out Dictionary<string, object> parameters)
        {
            parameters = _parameters;
            return Recurse(expression.Body, true);
        }

        private string Recurse(Expression expression, bool isUnary = false, bool quote = true)
        {
            if (expression is UnaryExpression)
            {
                var unary = (UnaryExpression)expression;
                var right = Recurse(unary.Operand, true);
                return "(" + NodeTypeToString(unary.NodeType, right == "NULL") + " " + right + ")";
            }
            if (expression is MemberExpression)
            {
                var member = (MemberExpression)expression;

                if (member.Member is PropertyInfo)
                {
                    var property = (PropertyInfo)member.Member;

                    if (property.PropertyType == typeof(bool))
                    {
                        var colName = property.Name;
                        return $"([{colName}])";
                    }
                }

                if (member.Member is FieldInfo)
                {
                    var fieldValue = GetValue(member);

                }
            }
            if (expression is BinaryExpression)
            {
                var body = (BinaryExpression)expression;

                var right = "";
                if (body.Right is BinaryExpression)
                {
                    right = Recurse(body.Right);
                }
                else if (body.Right is MethodCallExpression)
                {
                    var methodCall = (MethodCallExpression)body.Right;
                    // LIKE queries:
                    if (methodCall.Method == typeof(string).GetMethod("StartsWith", new[] { typeof(string) }))
                    {
                        var expressionName = GetMemberExpressionName((MemberExpression)methodCall.Object);
                        var parameterName = GetParameterName(expressionName);
                        var value = $"'{GetExpressionValue(methodCall.Arguments[0])}%'";
                        _parameters.Add(parameterName, value);
                        right = $"([{expressionName}] LIKE @{parameterName})";
                    }
                    if (methodCall.Method == typeof(string).GetMethod("EndsWith", new[] { typeof(string) }))
                    {
                        var expressionName = GetMemberExpressionName((MemberExpression)methodCall.Object);
                        var parameterName = GetParameterName(expressionName);
                        var value = $"'%{GetExpressionValue(methodCall.Arguments[0])}'";
                        _parameters.Add(parameterName, value);
                        right = $"([{expressionName}] LIKE @{parameterName})";
                    }
                    // IN queries:
                    if (methodCall.Method.Name == "Contains")
                    {
                        if (methodCall.Method == typeof(string).GetMethod("Contains", new[] { typeof(string) }))
                        {
                            var expressionName = GetMemberExpressionName((MemberExpression)methodCall.Object);
                            var parameterName = GetParameterName(expressionName);
                            var value = $"'%{GetExpressionValue(methodCall.Arguments[0])}%'";
                            _parameters.Add(parameterName, value);
                            right = $"([{expressionName}] LIKE @{parameterName})";
                        }
                        else
                        {
                            Expression collection;
                            Expression property;
                            if (methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 2)
                            {
                                collection = methodCall.Arguments[0];
                                property = methodCall.Arguments[1];
                            }
                            else if (!methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 1)
                            {
                                collection = methodCall.Object;
                                property = methodCall.Arguments[0];
                            }
                            else
                            {
                                throw new Exception("Unsupported method call: " + methodCall.Method.Name);
                            }
                            var values = (IEnumerable)GetValue(collection);
                            var concated = "";

                            var expressionName = GetMemberExpressionName((MemberExpression)property);

                            foreach (var value in values)
                            {
                                var parameterName = GetParameterName(expressionName);
                                _parameters.Add(parameterName, value);
                                concated += "@" + parameterName + ",";
                            }

                            if (concated == "")
                            {
                                right = ValueToString(false, true, false);
                            }
                            else
                            {
                                if (concated.EndsWith(","))
                                {
                                    concated = concated.Remove(concated.Length - 1, 1);
                                }


                                right = $"([{expressionName}] IN ({concated}))";
                            }
                        }
                    }
                    return "(" + Recurse(body.Left) + " " + NodeTypeToString(body.NodeType, right == "NULL") + " " + right + ")";
                }
                else
                {
                    if (body.Right is MemberExpression)
                    {
                        var member = (MemberExpression)body.Right;

                        if (member.Member is PropertyInfo)
                        {
                            var memberName = "";
                            var property = (PropertyInfo)member.Member;

                            if (property.PropertyType == typeof(bool))
                            {
                                memberName = property.Name;
                                return $"({Recurse(body.Left)} {NodeTypeToString(body.NodeType, right == "NULL")} [{memberName}])";
                            }
                            var value = GetValue(member);
                            var leftMember = (MemberExpression)body.Left;

                            memberName = GetMemberExpressionName(leftMember);

                            var parameterName = GetParameterName(memberName);
                            _parameters.Add(parameterName, value);
                            return $"([{memberName}] {NodeTypeToString(body.NodeType, right == "NULL")} @{parameterName})";
                        }

                        if (member.Member is FieldInfo)
                        {
                            var fieldValue = GetValue(member);
                            if (body.Left is MemberExpression)
                            {
                                var leftMember = (MemberExpression)body.Left;

                                var memberName = GetMemberExpressionName(leftMember);

                                var parameterName = GetParameterName(memberName);
                                _parameters.Add(parameterName, fieldValue);
                                return $"([{memberName}] {NodeTypeToString(body.NodeType, right == "NULL")} @{parameterName})";
                            }
                        }
                    }

                    if (body.Right is ConstantExpression)
                    {
                        if (body.Left is UnaryExpression)
                        {
                            var member = (UnaryExpression)body.Left;
                            var memberName = GetMemberExpressionName((MemberExpression)member.Operand);
                            var parameterName = GetParameterName(memberName);
                            var constant = (ConstantExpression)body.Right;
                            _parameters.Add(parameterName, constant.Value);
                            return $"([{memberName}] {NodeTypeToString(body.NodeType, right == "NULL")} @{parameterName})";
                        }

                        if (body.Left is MemberExpression)
                        {
                            var member = (MemberExpression)body.Left;

                            var memberName = GetMemberExpressionName(member);
                            var parameterName = GetParameterName(memberName);
                            var constant = (ConstantExpression)body.Right;
                            _parameters.Add(parameterName, constant.Value);
                            return $"([{memberName}] {NodeTypeToString(body.NodeType, right == "NULL")} @{parameterName})";
                        }
                    }

                    if (body.Right is UnaryExpression)
                    {
                        var value = GetValue(body.Right);
                        if (body.Left is UnaryExpression)
                        {
                            var member = (UnaryExpression)body.Left;
                            var memberName = GetMemberExpressionName((MemberExpression)member.Operand);
                            var parameterName = GetParameterName(memberName);
                            _parameters.Add(parameterName, value);
                            return $"([{memberName}] {NodeTypeToString(body.NodeType, right == "NULL")} @{parameterName})";
                        }

                        if (body.Left is MemberExpression)
                        {
                            var member = (MemberExpression)body.Left;

                            var memberName = GetMemberExpressionName(member);
                            var parameterName = GetParameterName(memberName);
                            _parameters.Add(parameterName, value);
                            return $"([{memberName}] {NodeTypeToString(body.NodeType, right == "NULL")} @{parameterName})";
                        }
                    }

                    if (body.Right is NewExpression)
                    {
                        var fieldValue = GetValue(body.Right);
                        if (body.Left is MemberExpression)
                        {
                            var leftMember = (MemberExpression)body.Left;

                            var memberName = GetMemberExpressionName(leftMember);

                            var parameterName = GetParameterName(memberName);
                            _parameters.Add(parameterName, fieldValue);
                            return $"([{memberName}] {NodeTypeToString(body.NodeType, right == "NULL")} @{parameterName})";
                        }
                    }
                }

                return "(" + Recurse(body.Left) + " " + NodeTypeToString(body.NodeType, right == "NULL") + " " + right + ")";
            }
            if (expression is MethodCallExpression)
            {
                var methodCall = (MethodCallExpression)expression;
                // LIKE queries:
                if (methodCall.Method == typeof(string).GetMethod("Contains", new[] { typeof(string) }))
                {
                    var expressionName = GetMemberExpressionName((MemberExpression)methodCall.Object);
                    var parameterName = GetParameterName(expressionName);
                    var value = $"%{GetExpressionValue(methodCall.Arguments[0])}%";
                    _parameters.Add(parameterName, value);
                    return $"([{expressionName}] LIKE @{parameterName})";
                }
                if (methodCall.Method == typeof(string).GetMethod("StartsWith", new[] { typeof(string) }))
                {
                    var expressionName = GetMemberExpressionName((MemberExpression)methodCall.Object);
                    var parameterName = GetParameterName(expressionName);
                    var value = $"{GetExpressionValue(methodCall.Arguments[0])}%";
                    _parameters.Add(parameterName, value);
                    return $"([{expressionName}] LIKE @{parameterName})";
                }
                if (methodCall.Method == typeof(string).GetMethod("EndsWith", new[] { typeof(string) }))
                {
                    var expressionName = GetMemberExpressionName((MemberExpression)methodCall.Object);
                    var parameterName = GetParameterName(expressionName);
                    var value = $"%{GetExpressionValue(methodCall.Arguments[0])}";
                    _parameters.Add(parameterName, value);
                    return $"([{expressionName}] LIKE @{parameterName})";
                }
                // IN queries:
                if (methodCall.Method.Name == "Contains")
                {
                    Expression collection;
                    Expression property;
                    if (methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 2)
                    {
                        collection = methodCall.Arguments[0];
                        property = methodCall.Arguments[1];
                    }
                    else if (!methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 1)
                    {
                        collection = methodCall.Object;
                        property = methodCall.Arguments[0];
                    }
                    else
                    {
                        throw new Exception("Unsupported method call: " + methodCall.Method.Name);
                    }
                    var values = (IEnumerable)GetValue(collection);
                    var concated = "";
                    var expressionName = GetMemberExpressionName((MemberExpression)property);

                    foreach (var value in values)
                    {
                        var parameterName = GetParameterName(expressionName);
                        _parameters.Add(parameterName, value);
                        concated += "@" + parameterName + ",";
                    }
                    if (concated.EndsWith(","))
                    {
                        concated = concated.Remove(concated.Length - 1, 1);
                    }
                    if (concated == "")
                    {
                        return ValueToString(false, true, false);
                    }

                    return $"([{expressionName}] IN ({concated}))";
                }
            }
            throw new Exception("Unsupported expression: " + expression.GetType().Name);
        }

        public string ValueToString(object value, bool isUnary, bool quote)
        {
            if (value is bool)
            {
                if (isUnary)
                {
                    return (bool)value ? "(1=1)" : "(1=0)";
                }
                return (bool)value ? "1" : "0";
            }
            if (value is string)
            {
                return $"'{value}'";
            }
            return value.ToString();
        }

        private static bool IsEnumerableType(Type type)
        {
            return type
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private static object GetValue(Expression member)
        {
            // source: http://stackoverflow.com/a/2616980/291955
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private static object NodeTypeToString(ExpressionType nodeType, bool rightIsNull)
        {
            switch (nodeType)
            {
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.And:
                    return "&";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Equal:
                    return rightIsNull ? "IS" : "=";
                case ExpressionType.ExclusiveOr:
                    return "^";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Negate:
                    return "-";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.Subtract:
                    return "-";
            }
            throw new Exception($"Unsupported node type: {nodeType}");
        }

        private string GetParameterName(string nameInExpression)
        {
            var nameCount = _parameters.Keys.Count(x => x.StartsWith(nameInExpression));
            return $"{nameInExpression}_{nameCount}";
        }

        private string GetMemberExpressionName(MemberExpression expression)
        {
            if (expression.Member is PropertyInfo)
            {
                var property = (PropertyInfo)expression.Member;
                return property.Name;
            }

            if (expression.Member is FieldInfo)
            {
                var field = (FieldInfo)expression.Member;
                return field.Name;
            }

            throw new Exception("Unsupported expression: " + expression.GetType().Name);
        }

        private static object GetExpressionValue(Expression expression)
        {
            if (expression is ConstantExpression)
            {
                var member = (ConstantExpression)expression;
                return member.Value;
            }

            if (expression is MemberExpression)
            {
                return GetValue(expression);
            }

            throw new Exception("Unsupported expression: " + expression.GetType().Name);
        }
    }
}