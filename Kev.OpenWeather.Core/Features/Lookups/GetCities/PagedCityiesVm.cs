using Kev.OpenWeather.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Features.Lookups.GetCities
{
    public class PagedCityiesVm
    {
        public int Page { get; set; }

        public int Count { get; set; }

        public int Size { get; set; }

        public ICollection<WorldCityDto> Cities { get; set; }
    }
}
