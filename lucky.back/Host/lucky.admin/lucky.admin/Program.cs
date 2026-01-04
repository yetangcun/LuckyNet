using Common.CoreLib.Extension.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddSwaggerExt(builder.Configuration); // ÃÌº”swagger≈‰÷√

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwaggerExt();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
