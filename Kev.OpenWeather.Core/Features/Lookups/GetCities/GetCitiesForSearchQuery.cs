using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Features.Lookups.GetCities
{
    public class GetCitiesForSearchQuery :  IRequest<PagedCityiesVm>
    {
        public string Search { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }
    }
}
