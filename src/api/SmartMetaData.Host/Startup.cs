using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using SmartMetaData.Host.Converters;
using SmartMetaData.Infrastructure.Options;
using SmartMetaData.Infrastructure.Services;

namespace SmartMetaData.Host;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RpcOptions>(configuration.GetSection("RpcOptions"));

        services.AddScoped<IBlockService, BlockService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new BigIntegerConverter());
            options.JsonSerializerOptions.Converters.Add(new HexBigIntegerConverter());
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IEndpointRouteBuilder routeBuilder, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        routeBuilder.MapControllers();
    }
}
