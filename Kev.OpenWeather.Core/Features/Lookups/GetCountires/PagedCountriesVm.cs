using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Features.Lookups.GetCountires
{
    public class PagedCountriesVm
    {
        public int Page { get; set; }

        public int Count { get; set; }

        public int Size { get; set; }

        public ICollection<CountryDto> Countries { get; set; }
    }
}
