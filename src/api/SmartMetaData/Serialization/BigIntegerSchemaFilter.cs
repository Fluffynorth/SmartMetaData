using System.Numerics;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartMetaData.Serialization;

public class BigIntegerSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(BigInteger) || context.Type == typeof(BigInteger?))
        {
            schema.Type = "integer";
            schema.Format = "int64";
            schema.Properties = new Dictionary<string, OpenApiSchema>();
        }
    }
}
