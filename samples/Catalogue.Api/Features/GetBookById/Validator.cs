using Bolt.Endeavor.Extensions.Bus.FluentValidation;
using Bolt.IocScanner.Attributes;
using Bookworm.Catalogue.Api.Contracts;
using FluentValidation;

namespace Bookworm.Catalogue.Api.Features.GetBookById;

[AutoBind]
internal sealed class Validator : FluentRequestValidator<GetBookByIdRequest> 
{
    public Validator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Id cannot be empty")
            .Must(x => x.StartsWith("bw-")).WithMessage("Book id is invalid");
        
    }
}