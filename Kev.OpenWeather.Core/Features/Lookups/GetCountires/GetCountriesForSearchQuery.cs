using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Features.Lookups.GetCountires
{
    public class GetCountriesForSearchQuery : IRequest<PagedCountriesVm>
    {
        public string Search { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }
    }

}
