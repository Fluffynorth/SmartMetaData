using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SmartMetaData.Host.Converters;
using SmartMetaData.Host.Exceptions;
using SmartMetaData.Host.Options;
using SmartMetaData.Infrastructure.Options;
using SmartMetaData.Infrastructure.Services;

namespace SmartMetaData.Host;

public class Startup
{
    private const string RpcOptionsSection = "RpcOptions";

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var rpcOptions = configuration.GetSection(RpcOptionsSection).Get<RpcOptions>();
        if (string.IsNullOrEmpty(rpcOptions.InfuraProjectId))
        {
            throw new InvalidConfigurationException("Infura project id is not set", $"{RpcOptionsSection}__{nameof(rpcOptions.InfuraProjectId)}");
        }

        services.AddSingleton<IOptions<RpcOptions>>(new Options<RpcOptions>(rpcOptions));

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
