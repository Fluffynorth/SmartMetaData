using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Serialization.ModelBinders.Providers;

public class AddressModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (context.Metadata.ModelType != typeof(Address))
            return null;

        return new BinderTypeModelBinder(typeof(AddressModelBinder));
    }
}
