using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Chris.Personnel.Management.API.Extensions
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chris.Personnel.Management.API", Version = "v1" });

                var xmlApiPath = Path.Combine(AppContext.BaseDirectory, "Chris.Personnel.Management.API.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlApiPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlViewModelPath = Path.Combine(AppContext.BaseDirectory, "Chris.Personnel.Management.ViewModel.xml");
                c.IncludeXmlComments(xmlViewModelPath);

                var xmlCommandPath = Path.Combine(AppContext.BaseDirectory, "Chris.Personnel.Management.UICommand.xml");
                c.IncludeXmlComments(xmlCommandPath);

                // 开启加权小锁
                //c.OperationFilter<AddResponseHeadersFilter>();
                //c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //// 在header中添加token，传递到后台
                //c.OperationFilter<SecurityRequirementsOperationFilter>();

                //// Jwt Bearer 认证，必须是 oauth2
                //c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //});
            });
        }
    }
}
