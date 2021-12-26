using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace SmartMetaData.Swagger.ApiDescriptionFilters;

public abstract class HidePropertiesApiDescriptionFilter<T> : IApiDescriptionFilter
{
    public void Apply(ApiDescriptionGroupCollection apiDescriptionGroupCollection)
    {
        var apiDescriptions = apiDescriptionGroupCollection.Items.SelectMany(x => x.Items).ToArray();
        foreach (var apiDescription in apiDescriptions)
        {
            var list = apiDescription.ParameterDescriptions.ToList();
            foreach (var parameterDescription in list)
            {
                var isPropertyOfSpecifiedType = parameterDescription.ModelMetadata != null &&
                                                parameterDescription.ModelMetadata.MetadataKind == ModelMetadataKind.Property &&
                                                parameterDescription.ModelMetadata.ContainerType == typeof(T);
                if (isPropertyOfSpecifiedType)
                    apiDescription.ParameterDescriptions.Remove(parameterDescription);
            }
        }
    }
}
