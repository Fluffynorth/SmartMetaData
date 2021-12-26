using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SmartMetaData.Swagger.ApiDescriptionFilters;

public interface IApiDescriptionFilter
{
    void Apply(ApiDescriptionGroupCollection apiDescriptionGroupCollection);
}
