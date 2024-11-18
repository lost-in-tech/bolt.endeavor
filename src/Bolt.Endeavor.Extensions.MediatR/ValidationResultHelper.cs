using FluentValidation.Results;

namespace Bolt.Endeavor.Extensions.MediatR;

public static class ValidationResultHelper
{
    public static MaySucceed ToMaySucceed(ValidationResult validationResult)
    {
        if (validationResult.IsValid) return MaySucceed.Ok();
        
        var errors = validationResult.Errors.Select(x => new Error(x.ErrorMessage, x.PropertyName, x.ErrorCode)).ToArray();

        return HttpResult.BadRequest(errors);
    }
}