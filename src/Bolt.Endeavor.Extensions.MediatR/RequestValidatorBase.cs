using FluentValidation;
using MediatR;

namespace Bolt.Endeavor.Extensions.MediatR;

public abstract class RequestValidatorBase<TRequest> 
    : AbstractValidator<TRequest>, IPipelineBehavior<TRequest, MaySucceed> 
    where TRequest : IRequest<MaySucceed>
{
    public virtual Task<MaySucceed> Handle(TRequest request, RequestHandlerDelegate<MaySucceed> next, CancellationToken cancellationToken)
    {
        var validationResult = ValidationResultHelper.ToMaySucceed(Validate(request));

        if (validationResult.IsFailed)
        {
            return Task.FromResult<MaySucceed>(validationResult.Failure);
        }

        return next();
    }
}

public abstract class RequestValidatorBase<TRequest,TResponse> 
    : AbstractValidator<TRequest>, IPipelineBehavior<TRequest, MaySucceed<TResponse>> 
    where TRequest : IRequest<MaySucceed<TResponse>>
{
    public Task<MaySucceed<TResponse>> Handle(TRequest request, RequestHandlerDelegate<MaySucceed<TResponse>> next, CancellationToken cancellationToken)
    {
        var validationResult = ValidationResultHelper.ToMaySucceed(Validate(request));

        if (validationResult.IsFailed)
        {
            return Task.FromResult<MaySucceed<TResponse>>(validationResult.Failure);
        }
        
        return next();
    }
}