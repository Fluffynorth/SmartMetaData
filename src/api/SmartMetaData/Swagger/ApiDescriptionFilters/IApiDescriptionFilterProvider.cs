namespace SmartMetaData.Swagger.ApiDescriptionFilters;

public interface IApiDescriptionFilterProvider
{
    IReadOnlyCollection<IApiDescriptionFilter> ApiDescriptionFilters { get; }
}
