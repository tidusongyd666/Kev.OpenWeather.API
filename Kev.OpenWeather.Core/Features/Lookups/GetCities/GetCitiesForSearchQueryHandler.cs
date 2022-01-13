using Kev.OpenWeather.Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kev.OpenWeather.Core.Features.Lookups.GetCities
{
    public class GetCitiesForSearchQueryHandler : IRequestHandler<GetCitiesForSearchQuery, PagedCityiesVm>
    {
        public async Task<PagedCityiesVm> Handle(GetCitiesForSearchQuery request, CancellationToken cancellationToken)
        {
            var list = string.IsNullOrEmpty(request.Search) ?
                ConfigManager.cityLookups
                .Skip((request.Page - 1) * request.Size).Take(request.Size).
                ToList() 
                :
                ConfigManager.cityLookups.Where(v => v.name.Contains(request.Search, StringComparison.OrdinalIgnoreCase))
                .Skip((request.Page - 1) * request.Size).Take(request.Size).
                ToList();


            var count = string.IsNullOrEmpty(request.Search) ?
                    ConfigManager.cityLookups.Count()
                    :
                    ConfigManager.cityLookups.Where(v => v.name.Contains(request.Search, StringComparison.OrdinalIgnoreCase)).Count();

            return new PagedCityiesVm() { Count = count, Cities = list, Page = request.Page, Size = request.Size };
        }
    }
}
