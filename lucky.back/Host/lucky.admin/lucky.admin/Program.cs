using Serilog;
using System.Net;
using Lucky.BaseService;
using lucky.admin.Extensions;
using System.Text.Encodings.Web;
using Common.CoreLib.Extension.Common;
using lucky.admin.Extensions.Filters;
using Microsoft.AspNetCore.HttpOverrides;
using Lucky.SysService;
using Lucky.PrtclService;

var bld = WebApplication.CreateBuilder(args);

// Add services to the container.
bld.Services.AddControllers(c =>
{
    c.Filters.Add<GlbFilter>();
    c.RespectBrowserAcceptHeader = true;
}).AddJsonOptions(opts =>  // 配置序列化 System.Text.Json
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = null; // 原样输出
    opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;  // 大小写不敏感
    opts.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss")); // 设置时间格式

    opts.JsonSerializerOptions.Converters.Add(new BoolJsonConverter()); // 设置bool获取格式
    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; // 忽略循环引用

    // 不使用驼峰样式的key
    opts.JsonSerializerOptions.DictionaryKeyPolicy = null;

    // 获取或设置要在转义字符串时使用的编码器
    opts.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
bld.Services.BaseInitLoad(bld.Configuration);
bld.Services.AddSwaggerExt(bld.Configuration); // 添加swagger配置
bld.Services.SysModuleLoad(bld.Configuration);  // 系统管理模块
bld.Services.PrtclModuleLoad(bld.Configuration); // 协议管理模块

bld.Host.UseSerilog((context, logger) => // 采用serilog日志
{
    logger.ReadFrom.Configuration(context.Configuration);
    logger.Enrich.FromLogContext();
});

// 配置信任代理
bld.Services.Configure<ForwardedHeadersOptions>(options =>
{
    // options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
    options.ForwardedHeaders = ForwardedHeaders.All;

    // 清除默认的已知代理
    options.KnownProxies.Clear();

    // 添加你的Nginx服务器IP地址
    // 如果是本地开发，可能是 127.0.0.1
    // 如果是Docker环境，可能是容器IP
    // 如果是服务器部署，是Nginx服务器的实际IP
    var proxyIp = new string[] { "8.155.173.34", "127.0.0.1", "localhost", "csaiot.aiuni.cn" };
    foreach (var ip in proxyIp)
    {
        if (IPAddress.TryParse(ip, out var ipAddress))
            options.KnownProxies.Add(ipAddress);
    }
});

var allowedCors = "AllowedCors";
var allowedOrigins = bld.Configuration.GetSection("CommonCfg:AllowedOrigins").Value?.Split(',') ?? [];
bld.Services.AddCors(opt =>
{
    opt.AddPolicy(allowedCors, policy =>
    {
        policy.WithOrigins(allowedOrigins).
        AllowAnyHeader().
        AllowAnyMethod().
        AllowCredentials().
        WithExposedHeaders("fresh_token");
    });
});

var app = bld.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwaggerExt();
}

app.UseForwardedHeaders(); // 开启反向代理

app.UseCors(allowedCors);  // app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
