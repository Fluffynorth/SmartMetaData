using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SmartMetaData.Swagger.ApiDescriptionFilters;

namespace SmartMetaData.Swagger;

public class CustomApiDescriptionProvider : IApiDescriptionGroupCollectionProvider
{
    private readonly ApiDescriptionGroupCollectionProvider _innerProvider;
    private readonly IApiDescriptionFilterProvider _filterProvider;

    public CustomApiDescriptionProvider(
        IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
        IEnumerable<IApiDescriptionProvider> apiDescriptionProviders,
        IApiDescriptionFilterProvider filterProvider)
    {
        _innerProvider = new ApiDescriptionGroupCollectionProvider(actionDescriptorCollectionProvider, apiDescriptionProviders);
        _filterProvider = filterProvider;
    }

    private ApiDescriptionGroupCollection _apiDescriptionGroups;

    public ApiDescriptionGroupCollection ApiDescriptionGroups => _apiDescriptionGroups ??= GetCollection();

    private ApiDescriptionGroupCollection GetCollection()
    {
        var apiDescriptionGroups = _innerProvider.ApiDescriptionGroups;
        var filters = _filterProvider?.ApiDescriptionFilters ?? Array.Empty<IApiDescriptionFilter>();

        foreach (var filter in filters)
        {
            filter.Apply(apiDescriptionGroups);
        }

        return apiDescriptionGroups;
    }
}
