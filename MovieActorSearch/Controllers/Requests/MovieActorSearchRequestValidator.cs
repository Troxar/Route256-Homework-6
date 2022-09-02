using FluentValidation;

namespace MovieActorSearch.Controllers.Requests;

public sealed class MovieActorSearchRequestValidator : AbstractValidator<MovieActorSearchRequest>
{
    public MovieActorSearchRequestValidator()
    {
        RuleFor(x => x.Actor1)
            .NotEmpty()
            .WithMessage("Actor1 mush be specified");
        RuleFor(x => x.Actor2)
            .NotEmpty()
            .WithMessage("Actor2 mush be specified");
    }
}