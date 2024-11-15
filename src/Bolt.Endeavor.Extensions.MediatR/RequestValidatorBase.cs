using FluentValidation;
using MediatR;

namespace Bolt.Endeavor.Extensions.MediatR;

public abstract class RequestValidatorBase<TRequest> 
    : AbstractValidator<TRequest>, IPipelineBehavior<TRequest, MaySucceed> 
    where TRequest : IRequest<MaySucceed>
{
    public Task<MaySucceed> Handle(TRequest request, RequestHandlerDelegate<MaySucceed> next, CancellationToken cancellationToken)
    {
        var validationResult = Validate(request);

        if (validationResult.IsValid) return next();

        var errors = validationResult.Errors.Select(x => new Error(x.ErrorMessage, x.PropertyName, x.ErrorCode)).ToArray();

        return Task.FromResult<MaySucceed>(HttpResult.BadRequest(errors));
    }
}

public abstract class RequestValidatorBase<TRequest,TResponse> 
    : AbstractValidator<TRequest>, IPipelineBehavior<TRequest, MaySucceed<TResponse>> 
    where TRequest : IRequest<MaySucceed<TResponse>>
{
    public Task<MaySucceed<TResponse>> Handle(TRequest request, RequestHandlerDelegate<MaySucceed<TResponse>> next, CancellationToken cancellationToken)
    {
        var validationResult = Validate(request);

        if (validationResult.IsValid) return next();

        var errors = validationResult.Errors.Select(x => new Error(x.ErrorMessage, x.PropertyName, x.ErrorCode)).ToArray();

        return Task.FromResult<MaySucceed<TResponse>>(HttpResult.BadRequest(errors));
    }
}