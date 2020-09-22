using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Chris.Personnel.Management.API.Extensions
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Chris.Personnel.Management.API", Version = "v1" });

                var xmlApiPath = Path.Combine(AppContext.BaseDirectory, "Chris.Personnel.Management.API.xml");//这个就是刚刚配置的xml文件名
                options.IncludeXmlComments(xmlApiPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlViewModelPath = Path.Combine(AppContext.BaseDirectory, "Chris.Personnel.Management.ViewModel.xml");
                options.IncludeXmlComments(xmlViewModelPath);

                var xmlCommandPath = Path.Combine(AppContext.BaseDirectory, "Chris.Personnel.Management.UICommand.xml");
                options.IncludeXmlComments(xmlCommandPath);

                // 开启加权小锁
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                options.OperationFilter<SecurityRequirementsOperationFilter>();

                //// Jwt Bearer 认证，必须是 oauth2
                //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //});

                // 接入identityserver4认证
                //options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Flow = "implicit", // 只需通过浏览器获取令牌（适用于swagger）
                //    AuthorizationUrl = "http://localhost:5000/connect/authorize",//获取登录授权接口
                //    Scopes = new Dictionary<string, string> {
                //        { "swagger_api", "同意swagger_api 的访问权限" }//指定客户端请求的api作用域。 如果为空，则客户端无法访问
                //    }
                //});
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:5004/connect/authorize", UriKind.Absolute),
                            Scopes = new Dictionary<string, string> {
                                {
                                    "Chris.Personnel.Management.API",
                                    "请选择授权API"
                                }
                            }
                        }
                    }
                });

                //options.OperationFilter<AuthorizeCheckOperationFilter>(); // 添加IdentityServer4认证过滤
            });
        }
    }

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //获取是否添加登录特性
            //策略名称映射到范围
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "未经授权" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "禁止访问" });

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = requiredScopes.ToList()
                    }
                };
            }
        }
    }
}
