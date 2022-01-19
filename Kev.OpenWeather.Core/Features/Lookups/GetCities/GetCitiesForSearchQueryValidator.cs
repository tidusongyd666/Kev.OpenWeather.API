using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Features.Lookups.GetCities
{
    public class GetCitiesForSearchQueryValidator : AbstractValidator<GetCitiesForSearchQuery>
    {
        public GetCitiesForSearchQueryValidator()
        {
            RuleFor(s => s.Search)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(s => s.Page)
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be greater than or equal to one.");

            RuleFor(s => s.Size)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");

        }
    }
}
