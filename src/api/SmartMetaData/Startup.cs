using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SmartMetaData.Exceptions;
using SmartMetaData.Extensions;
using SmartMetaData.Options;
using SmartMetaData.Serialization.Converters;
using SmartMetaData.Serialization.ModelBinders.Providers;
using SmartMetaData.Services;
using SmartMetaData.Swagger;
using SmartMetaData.Swagger.ApiDescriptionFilters;
using SmartMetaData.Swagger.SchemaFilters;

namespace SmartMetaData;

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

        services
            .AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new AddressModelBinderProvider());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.JsonSerializerOptions.Converters.Add(new BigIntegerConverter());
                options.JsonSerializerOptions.Converters.Add(new HexBigIntegerConverter());
                options.JsonSerializerOptions.Converters.Add(new AddressConverter());
            });

        services.AddEndpointsApiExplorer();
        services.AddSingleton<IApiDescriptionFilter, AddressApiDescriptionFilter>();
        services.AddSingleton<IApiDescriptionFilter, BigIntegerApiDescriptionFilter>();
        services.AddSingleton<IApiDescriptionFilterProvider, ApiDescriptionFilterProvider>();
        services.ReplaceSingleton<IApiDescriptionGroupCollectionProvider, CustomApiDescriptionProvider>();

        services.AddSwaggerGen(x =>
        {
            x.SchemaFilter<BigIntegerSchemaFilter>();
        });
    }

    public void Configure(IApplicationBuilder app, IEndpointRouteBuilder routeBuilder, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        routeBuilder.MapControllers();
    }
}
