using System.Text.Json.Serialization;
using SmartMetaData.Host.Converters;
using SmartMetaData.Infrastructure.Options;
using SmartMetaData.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<RpcOptions>(builder.Configuration.GetSection("RpcOptions"));

builder.Services.AddScoped<IBlockService, BlockService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new BigIntegerConverter());
    options.JsonSerializerOptions.Converters.Add(new HexBigIntegerConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
