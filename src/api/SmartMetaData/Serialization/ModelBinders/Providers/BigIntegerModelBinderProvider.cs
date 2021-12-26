using System.Numerics;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace SmartMetaData.Serialization.ModelBinders.Providers;

public class BigIntegerModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (context.Metadata.ModelType != typeof(BigInteger))
            return null;

        return new BinderTypeModelBinder(typeof(BigIntegerModelBinder));
    }
}
