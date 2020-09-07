using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static  class SwaggerConfig
{
   public static void AddSwagger(this IServiceCollection services,string PlatformName ,string xmlName)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var basePath = AppContext.BaseDirectory;
        var ApiName = PlatformName+ " system";
        services.AddSwaggerGen(c =>
        {
            string version = "1";
            c.SwaggerDoc(version, new OpenApiInfo
            {
                Version = version,
                Title = $"{ApiName} 接口文档——Netcore 3.1",
                Description = $"{ApiName} HTTP API "
            });
            c.OrderActionsBy(o => o.RelativePath);
            //就是这里
            var xmlPath = Path.Combine(basePath, xmlName+".xml");
            c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
            // Token绑定到ConfigureServices
            c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
            {
                Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                Name = "Authorization",//jwt默认的参数名称
                In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                Type = SecuritySchemeType.ApiKey,
                Scheme = "http"
            });
        });
    }
    public static void UseSwaggerSetup(this IApplicationBuilder app)
    {
        app.UseSwagger(c => c.SerializeAsV2 = true);
        app.UseSwaggerUI(c =>
        {

            //根据版本名称倒序 遍历展示
            var ApiName = "";
            var version = "1";
            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
        });

    }

}

