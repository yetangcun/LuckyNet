using Microsoft.OpenApi;
using Common.CoreLib.Model.Enum;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.CoreLib.Extension.Common
{
    /// <summary>
    /// swagger扩展
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// 添加swagger扩展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg"></param>
        public static void AddSwaggerExt(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddSwaggerGen(opt =>
            {
                typeof(ModuleApiType).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    // 获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(ModuleApiInfo), false).OfType<ModuleApiInfo>().FirstOrDefault();
                    opt.SwaggerDoc(f.Name, new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version,
                        Description = info?.Description
                    });
                });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Value: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                opt.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("oauth2", doc),
                        ["readAccess", "writeAccess"]
                    }
                });

                // 给Swagger接口添加注释说明
                var xmls = (cfg.GetSection("CommonCfg:ApiArrXmls").Value?.Split(',')) ?? [];
                string[] xmlFileArrs = xmls; // { "Plat.NetServer.xml", "Plat.General.Model.xml", "Models.SysModel.xml" };
                foreach (var xmlFile in xmlFileArrs)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                        opt.IncludeXmlComments(xmlPath, true);
                }
            });
        }

        /// <summary>
        /// 采用swagger配置
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExt(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                typeof(ModuleApiType).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(ModuleApiInfo), false).OfType<ModuleApiInfo>().FirstOrDefault();
                    opt.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);

                });
            });
        }
    }
}
