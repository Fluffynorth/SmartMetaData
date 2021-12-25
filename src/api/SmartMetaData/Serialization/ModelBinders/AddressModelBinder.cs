using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Serialization.ModelBinders;

public class AddressModelBinder : IModelBinder
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

        var addressResult = Address.Create(value);
        if (addressResult.IsFailure)
        {
            bindingContext.ModelState.TryAddModelError(modelName, addressResult.Error);
            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(addressResult.Value);
        return Task.CompletedTask;
    }
}
