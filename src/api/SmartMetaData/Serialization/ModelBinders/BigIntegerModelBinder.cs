using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartMetaData.Utils;

namespace SmartMetaData.Serialization.ModelBinders;

public class BigIntegerModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        var bigIntegerResult = ParseUtils.ParseBigInteger(value);
        if (bigIntegerResult.IsFailure)
        {
            bindingContext.ModelState.TryAddModelError(modelName, bigIntegerResult.Error);
            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(bigIntegerResult.Value);
        return Task.CompletedTask;
    }
}
