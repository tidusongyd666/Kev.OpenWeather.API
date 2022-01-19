using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Features.Lookups.GetCountires
{
    public class GetCountriesForSearchQueryValidator : AbstractValidator<GetCountriesForSearchQuery>
    {
        public GetCountriesForSearchQueryValidator()
        {
            RuleFor(s => s.Search)
               .MaximumLength(10).WithMessage("{PropertyName} must not exceed 10 characters.");

            RuleFor(s => s.Page)
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be greater than or equal to one.");

            RuleFor(s => s.Size)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
        }
    }
}
