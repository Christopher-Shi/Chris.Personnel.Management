using Autofac;
using Autofac.Extensions.DependencyInjection;
using Chris.Personnel.Management.API.Extensions;
using Chris.Personnel.Management.Common.EntityModel;
using Chris.Personnel.Management.Common.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Chris.Personnel.Management.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettings(Configuration));

            services.AddAutoMapperSetup();

            services.AddControllers();

            services.AddSwaggerSetup();

            //JWT ��֤
            //services.AddAuthenticationSetup();

            //IdentityServer4 ��֤
            services.AddAuthorizationIds4Setup();

            //TODO:_httpContextAccessor.HttpContext.User.Identity.Name IS NULL
            services.AddHttpContextSetup();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = "https://localhost:5004";

            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = false
            //        };

            //        options.Audience = "Chris.Personnel.Management.API";
            //    });

            //�������
            services.AddCors(options =>
            {
                options.AddPolicy("Open",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddRouting(options =>
            {
                // Ĭ�����ɵ� URL ��ַ��ΪȫСдģʽ
                options.LowercaseUrls = true;
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder.RegisterModule(new AutofacModuleRegister());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //�������(˳�����Ҫ, �������UseAuthentication��UseAuthorization��ǰ��)
            app.UseCors("Open");

            // �ȿ�����֤
            app.UseAuthentication();

            // Ȼ������Ȩ�м��
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chris.Personnel.Management.API V1");
                options.RoutePrefix = "";
                options.OAuthClientId("swagger client");//�ͷ�������
                options.OAuthAppName("Swagger UI client"); // ����
            });

            //��ȡAutofac��Container
            DependencyResolverInitializer.Initialize(app.ApplicationServices.GetAutofacRoot());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
