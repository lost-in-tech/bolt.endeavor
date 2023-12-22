using FluentValidation;

namespace Bolt.MaySucceed.Extensions.Bus.FluentValidation;

public abstract class FluentRequestValidator<TRequest> : AbstractValidator<TRequest>, IRequestValidator<TRequest>
{
    public virtual int Priority => SystemPriorities.ValidationFilter;

    public virtual bool IsApplicable(IBusContextReader context, TRequest request) => true;

    public virtual async ValueTask<MaySucceed> Validate(IBusContextReader context, 
        TRequest input, 
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateAsync(input, cancellationToken);

        if (validationResult.IsValid) return MaySucceed.Ok();

        var errors = new Error[validationResult.Errors.Count];

        for(var index = 0; index < validationResult.Errors.Count; index++)
        {
            var valError = validationResult.Errors[index];
            errors[index] = new
            (
                Code: valError.ErrorCode, 
                Message: valError.ErrorMessage, 
                PropertyName: valError.PropertyName
            );
        }

        return errors;
    }
}
