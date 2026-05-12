using Serilog;
using MqttServerLib;
using MqttServerLib.Model;
using MqttServerHost.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfig();

var mqttSection = builder.Configuration.GetSection("MqttServerOption");
builder.Services.Configure<MqttServerOption>(mqttSection); // 添加MqttServerOption配置
// builder.Services.AddSingleton<IMqttServerExt, MqttServerExt>();

// 使用Serilog
builder.Host.UseSerilog((context, logger) => // 使用日志
{
    logger.ReadFrom.Configuration(context.Configuration);
    logger.Enrich.FromLogContext();
});

builder.MqttServerModuleLoad(builder.Configuration); // 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true) // app.Environment.IsDevelopment()
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseAuthorization();

app.MqttServerModuleInit(builder.Configuration);

app.MapControllers();

#region 初始化启动

//var mqttServr = app.Services.GetService<IMqttServerExt>();
//mqttServr!.StartAsync().GetAwaiter().GetResult();

#endregion

app.Run();
