using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChargerAstronomyShared.Contracts.Repositories
{
    using ChargerAstronomyShared.Contracts.Models;
    using ChargerAstronomyShared.Contracts.Streaming;
    using ChargerAstronomyShared.Domain.Equatorial;
    using System.Threading;

    public interface IStarRepository
    {
        Task<EquatorialStar> GetStarByIdAsync(int id);

        Task<PageResult<EquatorialStar>> GetAllAsync(PageRequest page);

        Task<PageResult<EquatorialStar>> QueryBySkyRegionAsync(SkyRegion skyRegion, PageRequest page);

        Task ProducePagesAsync(IInitializationQueue<PageResult<EquatorialStar>> queue, 
            PageRequest firstPage, CancellationToken cancellationToken = default);

    }
}
