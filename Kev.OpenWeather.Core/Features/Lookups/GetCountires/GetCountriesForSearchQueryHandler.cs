using Kev.OpenWeather.Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kev.OpenWeather.Core.Features.Lookups.GetCountires
{
    public class GetCountriesForSearchQueryHandler : IRequestHandler<GetCountriesForSearchQuery, PagedCountriesVm>
    {
        public async Task<PagedCountriesVm> Handle(GetCountriesForSearchQuery request, CancellationToken cancellationToken)
        {
            List<CountryDto> distinctCountires = ConfigManager.cityLookups.GroupBy(p => p.country).Select(g => new CountryDto() { Country = g.First().country ?? "" }).ToList();
          
            var list = string.IsNullOrEmpty(request.Search) ?
                distinctCountires.Skip((request.Page - 1) * request.Size).Take(request.Size).ToList()
               :
               distinctCountires.Where(v => v.Country.Contains(request.Search, StringComparison.OrdinalIgnoreCase))
               .Skip((request.Page - 1) * request.Size).Take(request.Size).
               ToList();


            var count = string.IsNullOrEmpty(request.Search) ?
                distinctCountires.Count()
                :
                distinctCountires.Where(v => v.Country.Contains(request.Search, StringComparison.OrdinalIgnoreCase)).Count();

            return new PagedCountriesVm() { Count = count, Countries = list, Page = request.Page, Size = request.Size };
        }
    }
}
