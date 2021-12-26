namespace SmartMetaData.Swagger.ApiDescriptionFilters;

public class ApiDescriptionFilterProvider : IApiDescriptionFilterProvider
{
    public ApiDescriptionFilterProvider(IServiceProvider serviceProvider)
    {
        ApiDescriptionFilters = serviceProvider.GetServices<IApiDescriptionFilter>().ToArray();
    }

    public IReadOnlyCollection<IApiDescriptionFilter> ApiDescriptionFilters { get; }
}
