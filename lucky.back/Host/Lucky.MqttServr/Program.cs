using Common.CoreLib.Extension.Common;
using Common.CoreLib.Model.Option;
using Prtcl.MqttServr;
using Serilog;
using System.Text.Encodings.Web;

var bld = WebApplication.CreateBuilder(args);

// Add services to the container.

bld.Services.AddControllers().AddJsonOptions(opts =>  // 配置序列化 System.Text.Json
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

// 使用Serilog
bld.Host.UseSerilog((cxt, logger) => // 使用日志
{
    logger.ReadFrom.Configuration(cxt.Configuration);
    logger.Enrich.FromLogContext();
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
bld.Services.AddSwaggerExt(bld.Configuration); // 添加swagger配置

var mqttServrCfg = bld.Configuration.GetSection("MqttServrOption");
bld.Services.Configure<MqttServrOption>(mqttServrCfg); // 添加MqttServrOption配置
bld.Services.AddSingleton<IMqttServr, MqttServr>();

var app = bld.Build();

// Configure the HTTP request pipeline.
if (true) // app.Environment.IsDevelopment()
{
    app.UseSwaggerExt();
}

app.UseAuthorization();

app.MapControllers();

#region 初始化启动

var mqttServr = app.Services.GetService<IMqttServr>();
mqttServr!.StartAsync().GetAwaiter().GetResult();

#endregion

app.Run();
